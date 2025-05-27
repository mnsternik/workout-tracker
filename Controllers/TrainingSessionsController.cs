using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.DTOs.Training.TrainingSession;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Utilities;

namespace WorkoutTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingSessionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TrainingSessionsController(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // GET: api/TrainingSessions
        [HttpGet]
        public async Task<ActionResult<PaginatedList<TrainingSessionReadDto>>> GetTrainingSessions(int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.TrainingSessions
                .Include(t => t.Exercises)
                .ThenInclude(te => te.Sets)
                .AsNoTracking();
                
            var dtoQuery = query.ProjectTo<TrainingSessionReadDto>(_mapper.ConfigurationProvider);  
                
            // Apply filtering or sorting 

            var paginatedList = await PaginatedList<TrainingSessionReadDto>.CreateAsync(dtoQuery, pageIndex, pageSize);

            return Ok(paginatedList);
        }


        // GET: api/TrainingSessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingSessionReadDto>> GetTrainingSession(int id)
        {
            var trainingSession = await _context.TrainingSessions
                .Include(t => t.Exercises)
                .ThenInclude(te => te.Sets)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainingSession == null)
            {
                return NotFound();
            }

            var trainingSessionDto = _mapper.Map<TrainingSessionReadDto>(trainingSession);

            return trainingSessionDto;
        }

        // PUT: api/TrainingSessions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingSession(int id, TrainingSession trainingSession)
        {
            if (id != trainingSession.Id)
            {
                return BadRequest();
            }

            _context.Entry(trainingSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingSessionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TrainingSessions
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TrainingSession>> PostTrainingSession(TrainingSessionCreateDto trainingSessionDto)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID not found in token");
            }

            var trainingSession = _mapper.Map<TrainingSession>(trainingSessionDto);
            trainingSession.UserId = userId; 

            _context.TrainingSessions.Add(trainingSession);
            await _context.SaveChangesAsync();

            var createdSessionDto = _mapper.Map<TrainingSessionReadDto>(trainingSession);

            return CreatedAtAction(nameof(GetTrainingSession), new { id = createdSessionDto.Id }, createdSessionDto);
        }

        // DELETE: api/TrainingSessions/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingSession(int id)
        {
            var trainingSession = await _context.TrainingSessions.FindAsync(id);
            if (trainingSession == null)
            {
                return NotFound();
            }

            _context.TrainingSessions.Remove(trainingSession);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingSessionExists(int id)
        {
            return _context.TrainingSessions.Any(e => e.Id == id);
        }
    }
}
