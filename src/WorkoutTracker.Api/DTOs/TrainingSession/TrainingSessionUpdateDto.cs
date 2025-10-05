using System.ComponentModel.DataAnnotations;
using WorkoutTracker.Api.DTOs.PerformedExercise;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.TrainingSession
{
    public record TrainingSessionUpdateDto
    {
        public int Id { get; init; }

        [Required]
        [MinLength(1, ErrorMessage = "Training session's name cannot be empty")]
        [MaxLength(100, ErrorMessage = "Training session's name must be shorter than 100 characters")]
        public string Name { get; init; } = string.Empty;

        [StringLength(1000)]
        public string? Note { get; init; }

        [Range(1, 5)]
        public DifficultyRating? DifficultyRating { get; init; }

        [Range(1, 1440)]
        public int? DurationMinutes { get; init; }

        [Required]
        public IList<PerformedExerciseUpdateDto> Exercises { get; init; } = [];
    }
}
