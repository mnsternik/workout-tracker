using System.ComponentModel.DataAnnotations;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.ExerciseDefinition
{
    public record ExerciseDefinitionCreateDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Exercise name cannot be empty")]
        [MaxLength(100, ErrorMessage = "Exercise name must be shorter than 100 characters")]
        public string Name { get; init; } = string.Empty;

        [Required]
        public string Description { get; init; } = string.Empty;

        [Url]
        public string? ImageUrl { get; init; }

        [Required]
        public ExerciseType ExerciseType { get; init; }

        [MinLength(1)]
        public IList<MuscleGroup> MuscleGroups { get; init; } = new List<MuscleGroup>();

        [Required]
        public Equipment Equipment { get; init; }

        [Required]
        public DifficultyLevel DifficultyLevel { get; init; }
    }
}
