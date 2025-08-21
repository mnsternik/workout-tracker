using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Api.DTOs.Exercise;
using WorkoutTracker.Api.Services.Exercises;

namespace WorkoutTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExercisesController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        // GET: api/Exercises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseReadDto>>> GetExercises([FromQuery] ExerciseQueryParameters queryParams)
        {
            return await _exerciseService.GetExercisesAsync(queryParams);
        }

        // GET: api/Exercises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseReadDto>> GetExercise(int id)
        {
            var exerciseDto = await _exerciseService.GetExerciseAsync(id);      
            return Ok(exerciseDto);
        }

        // PUT: api/Exercises/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercise(int id, ExerciseUpdateDto exerciseDto)
        {
            await _exerciseService.UpdateExerciseAsync(id, exerciseDto);
            return NoContent();
        }

        // POST: api/Exercises
        [HttpPost]
        public async Task<ActionResult<ExerciseReadDto>> PostExercise(ExerciseCreateDto exerciseDto)
        {
            var exercise = await _exerciseService.PostExerciseAsync(exerciseDto);
            return CreatedAtAction(nameof(GetExercise), new { id = exercise.Id }, exercise);
        }

        // DELETE: api/Exercises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            await _exerciseService.DeleteExercise(id);
            return NoContent();
        }
    }
}
