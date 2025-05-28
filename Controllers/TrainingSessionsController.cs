using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.DTOs.Training.TrainingSession;
using WorkoutTracker.Api.DTOs.TrainingSession.TrainingSession;
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
        public async Task<ActionResult<PaginatedList<TrainingSessionReadDto>>> GetTrainingSessions([FromQuery] TrainingSessionQueryParameters queryParams)
        {
            // Base IQueryable for training sessions
            var query = _context.TrainingSessions
                .Include(t => t.User)
                .Include(t => t.Exercises)
                    .ThenInclude(te => te.Exercise)
                        .ThenInclude(e => e.MuscleGroupsLinks)
                .Include(t => t.Exercises)
                    .ThenInclude(te => te.Sets)
                .AsNoTracking();

            // Filtering
            if (!string.IsNullOrEmpty(queryParams.UserId))
            {
                query = query.Where(t => t.UserId == queryParams.UserId);
            }

            if (!string.IsNullOrEmpty(queryParams.DisplayName))
            {
                query = query.Where(t => t.User.DisplayName.ToLower().Contains(queryParams.DisplayName.ToLower()));
            }

            if (queryParams.MinDifficulty.HasValue)
            {
                query = query.Where(t => t.DifficultyRating >= queryParams.MinDifficulty.Value);
            }

            if (queryParams.MaxDifficulty.HasValue)
            {
                query = query.Where(t => t.DifficultyRating <= queryParams.MaxDifficulty.Value);
            }

            if (queryParams.MinDurationMinutes.HasValue)
            {
                query = query.Where(t => t.DurationMinutes >= queryParams.MinDurationMinutes.Value);
            }

            if (queryParams.MaxDurationMinutes.HasValue)
            {
                query = query.Where(t => t.DurationMinutes <= queryParams.MaxDurationMinutes.Value);
            }

            if (queryParams.ExerciseNames != null && queryParams.ExerciseNames.Count > 0)
            {
                var lowerExerciseNames = queryParams.ExerciseNames.Select(e => e.ToLower()).ToList();
                query = query.Where(t => t.Exercises.Any(te => lowerExerciseNames.Contains(te.Exercise.Name.ToLower())));
            }

            if (queryParams.MuscleGroups != null && queryParams.MuscleGroups.Count > 0)
            {
                var lowerMuscleGroups = queryParams.MuscleGroups.Select(mg => mg.ToLower()).ToList();
                query = query.Where(t => t.Exercises.Any(te =>
                    te.Exercise.MuscleGroupsLinks.Any(mgl =>
                        lowerMuscleGroups.Contains(mgl.MuscleGroup.ToString().ToLower()))));
            }

            // TODO: Sorting




            var dtoQuery = query.ProjectTo<TrainingSessionReadDto>(_mapper.ConfigurationProvider);  

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
