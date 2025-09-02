using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Api.DTOs.TrainingSession;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Services.TrainingSessions;
using WorkoutTracker.Api.Utilities;

namespace WorkoutTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingSessionsController : ControllerBase
    {
        private readonly ITrainingSessionsService _trainingSessionsService;

        public TrainingSessionsController(ITrainingSessionsService trainingSessionsService)
        {
            _trainingSessionsService = trainingSessionsService;
        }

        // GET: api/TrainingSessions
        [HttpGet]
        public async Task<ActionResult<PaginatedList<TrainingSessionReadDto>>> GetTrainingSessions([FromQuery] TrainingSessionQueryParameters queryParams)
        {
            var trainingSessions = await _trainingSessionsService.GetTrainingSessionsAsync(queryParams);
            return Ok(trainingSessions);
        }

        // GET: api/TrainingSessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingSessionReadDto>> GetTrainingSession(int id)
        {
            var trainingSession = await _trainingSessionsService.GetTrainingSessionAsync(id);
            return Ok(trainingSession);
        }

        // PUT: api/TrainingSessions/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingSession(int id, [FromBody] TrainingSessionUpdateDto trainingSessionDto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _trainingSessionsService.UpdateTrainingSessionAsync(id, currentUserId, trainingSessionDto);
            return NoContent();
        }

        // POST: api/TrainingSessions
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TrainingSession>> PostTrainingSession(TrainingSessionCreateDto trainingSessionDto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdSessionDto = await _trainingSessionsService.PostTrainingSessionAsync(currentUserId, trainingSessionDto);
            return CreatedAtAction(nameof(GetTrainingSession), new { id = createdSessionDto.Id }, createdSessionDto);
        }

        // DELETE: api/TrainingSessions/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingSession(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _trainingSessionsService.DeleteTrainingSession(id, currentUserId);
            return NoContent();
        }
    }
}
