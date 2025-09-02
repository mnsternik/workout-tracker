using WorkoutTracker.Api.DTOs.PerformedExercise;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.TrainingSession
{
    public record TrainingSessionReadDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Note { get; init; }
        public DateTime CreatedAt { get; init; }
        public DifficultyRating? DifficultyRating { get; init; }
        public int? DurationMinutes { get; init; }
        public string UserId { get; init; } = string.Empty;
        public IList<PerformedExerciseReadDto> Exercises { get; init; } = new List<PerformedExerciseReadDto>();
    }
}
