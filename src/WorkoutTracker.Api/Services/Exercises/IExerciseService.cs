using WorkoutTracker.Api.DTOs.ExerciseDefinition;
using WorkoutTracker.Api.Utilities;

namespace WorkoutTracker.Api.Services.Exercises
{
    public interface IExerciseService
    {
        public Task<PaginatedList<ExerciseDefinitionReadDto>> GetExercisesAsync(ExerciseDefinitionQueryParameters queryParams);
        public Task<ExerciseDefinitionReadDto> GetExerciseAsync(int id);
        public Task<ExerciseDefinitionReadDto> PostExerciseAsync(ExerciseDefinitionCreateDto exerciseDto);
        public Task UpdateExerciseAsync(int id, ExerciseDefinitionUpdateDto exerciseDto);
        public Task DeleteExercise(int id); 
    }
}
