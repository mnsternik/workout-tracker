using System.ComponentModel.DataAnnotations;
using WorkoutTracker.Api.DTOs.Training.Exercise;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.TrainingSession.TrainingSession
{
    public class TrainingSessionUpdateDto
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
        public IList<TrainingExerciseCreateDto> Exercises { get; init; } = new List<TrainingExerciseCreateDto>();
    }
}
