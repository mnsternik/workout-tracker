using System.ComponentModel.DataAnnotations;
using WorkoutTracker.Api.DTOs.PerformedExercise;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.TrainingSession
{
    public record TrainingSessionCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; init; } = string.Empty;

        [StringLength(1000)]
        public string? Note { get; init; }

        [Range(1, 5)]
        public DifficultyRating? DifficultyRating { get; init; }

        [Range(1, 1440)]
        public int? DurationMinutes { get; init; }

        [Required]
        public IList<PerformedExerciseCreateDto> Exercises { get; init; } = new List<PerformedExerciseCreateDto>();
    }
}
