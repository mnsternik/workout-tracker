using System.ComponentModel.DataAnnotations;
using WorkoutTracker.Api.DTOs.Training.Exercise;

namespace WorkoutTracker.Api.DTOs.Training.TrainingSession
{
    public record TrainingSessionCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; init; } = string.Empty;

        [StringLength(1000)]
        public string? Notes { get; init; }

        [Range(1, 5)]
        public int? DifficultyRating { get; init; }

        [Range(1, 1440)]
        public int? EstimatedDurationMinutes { get; init; }

        [Required]
        public IList<TrainingExerciseCreateDto> Exercises { get; init; } = new List<TrainingExerciseCreateDto>();
    }
}
