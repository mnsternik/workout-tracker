using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.Exercise
{
    public record ExerciseDefinitionReadDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string? ImageUrl { get; init; }
        public ExerciseType ExerciseType { get; init; }
        public IList<ExerciseDefinitionMuscleGroupLinkDto> MuscleGroups { get; init; } = new List<ExerciseDefinitionMuscleGroupLinkDto>();
        public Equipment Equipment { get; init; }
        public DifficultyLevel DifficultyLevel { get; init; }
    }
}
