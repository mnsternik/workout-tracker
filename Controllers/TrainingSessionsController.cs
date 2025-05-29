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

            // Sorting
            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                bool isDescending = queryParams.SortOrder?.ToLower() == "desc";
                switch (queryParams.SortBy)
                {
                    case "name":
                        query = isDescending ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name);
                        break;
                    case "createdat":
                        query = isDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt);
                        break;
                    case "duration":
                        query = isDescending ? query.OrderByDescending(t => t.DurationMinutes) : query.OrderBy(t => t.DurationMinutes);
                        break;
                    case "difficulty":
                        query = isDescending ? query.OrderByDescending(t => t.DifficultyRating) : query.OrderBy(t => t.DifficultyRating);
                        break;
                    default:
                        query = isDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt);
                        break; 
                }
            }

            var dtoQuery = query.ProjectTo<TrainingSessionReadDto>(_mapper.ConfigurationProvider);  

            var paginatedList = await PaginatedList<TrainingSessionReadDto>.CreateAsync(dtoQuery, queryParams.PageNumber, queryParams.PageSize);

            return Ok(paginatedList);
        }


        // GET: api/TrainingSessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingSessionReadDto>> GetTrainingSession(int id)
        {
            var trainingSession = await _context.TrainingSessions
                .Include(t => t.User)
                .Include(t => t.Exercises)
                    .ThenInclude(te => te.Exercise)
                        .ThenInclude(e => e.MuscleGroupsLinks)
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
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingSession(int id, [FromBody] TrainingSessionUpdateDto trainingSessionDto)
        {
            var sessionToUpdate = await _context.TrainingSessions
                .Include(t => t.Exercises)
                    .ThenInclude(te => te.Sets)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (sessionToUpdate == null)
            {
                return BadRequest($"Training session with Id {id} not found"); 
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != sessionToUpdate.UserId)
            {
                return Forbid("You are not authorized to update this training session.");
            }

            // Update basic properties
            sessionToUpdate.Name = trainingSessionDto.Name;
            sessionToUpdate.Notes = trainingSessionDto.Notes;
            sessionToUpdate.DurationMinutes = trainingSessionDto.EstimatedDurationMinutes;
            sessionToUpdate.DifficultyRating = trainingSessionDto.DifficultyRating;

            // Clear existing exercises and sets
            _context.TrainingSets.RemoveRange(sessionToUpdate.Exercises.SelectMany(e => e.Sets));
            _context.TrainingExercises.RemoveRange(sessionToUpdate.Exercises);
            sessionToUpdate.Exercises.Clear();

            // Add new exercises and their sets
            foreach (var exerciseDto in trainingSessionDto.Exercises)
            {
                var exercise = _mapper.Map<TrainingExercise>(exerciseDto);
                exercise.TrainingSessionId = id;

                sessionToUpdate.Exercises.Add(exercise);
            }

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
                    return Conflict(new { message = "The training session was updated by another process. Please reload and try again." });
                }
            }

            return NoContent();
        }

        // POST: api/TrainingSessions
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TrainingSession>> PostTrainingSession(TrainingSessionCreateDto trainingSessionDto)
        {

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                return BadRequest("User ID not found in token");
            }

            var trainingSession = _mapper.Map<TrainingSession>(trainingSessionDto);
            trainingSession.UserId = currentUserId; 

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
