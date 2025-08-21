using WorkoutTracker.Api.DTOs.Exercise;
using WorkoutTracker.Api.Utilities;

namespace WorkoutTracker.Api.Services.Exercises
{
    public interface IExerciseService
    {
        public Task<PaginatedList<ExerciseReadDto>> GetExercisesAsync(ExerciseQueryParameters queryParams);
        public Task<ExerciseReadDto> GetExerciseAsync(int id);
        public Task<ExerciseReadDto> PostExerciseAsync(ExerciseCreateDto exerciseDto);
        public Task UpdateExerciseAsync(int id, ExerciseUpdateDto exerciseDto);
        public Task DeleteExercise(int id); 
    }
}
