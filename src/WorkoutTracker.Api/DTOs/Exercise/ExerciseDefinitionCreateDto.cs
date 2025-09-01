using System.ComponentModel.DataAnnotations;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.Exercise
{
    public record ExerciseDefinitionCreateDto
    {
        [Required]
        public string Name { get; init; } = string.Empty;

        [Required]
        public string Description { get; init; } = string.Empty;

        [Url]
        public string? ImageUrl { get; init; }

        [Required]
        public ExerciseType ExerciseType { get; init; }

        [MinLength(1)]
        public IList<ExerciseDefinitionMuscleGroupLinkDto> MuscleGroups { get; init; } = new List<ExerciseDefinitionMuscleGroupLinkDto>();

        [Required]
        public Equipment Equipment { get; init; }

        [Required]
        public DifficultyLevel DifficultyLevel { get; init; }
    }
}
