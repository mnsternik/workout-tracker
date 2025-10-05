using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.ExerciseDefinition
{
    public record ExerciseDefinitionReadDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string? ImageUrl { get; init; }
        public ExerciseType ExerciseType { get; init; }
        public IList<MuscleGroup> MuscleGroups { get; init; } = [];
        public Equipment Equipment { get; init; }
        public DifficultyLevel DifficultyLevel { get; init; }
    }
}
