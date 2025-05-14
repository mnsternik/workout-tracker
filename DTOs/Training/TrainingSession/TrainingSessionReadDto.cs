using WorkoutTracker.Api.DTOs.Training.Exercise;

namespace WorkoutTracker.Api.DTOs.Training.TrainingSession
{
    public record TrainingSessionReadDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Notes { get; init; }
        public DateTime CreatedAt { get; init; }
        public int? DifficultyRating { get; init; }
        public int? EstimatedDurationMinutes { get; init; }
        public string UserId { get; init; } = string.Empty;
        public IList<TrainingExerciseReadDto> Exercises { get; init; } = new List<TrainingExerciseReadDto>();
    }
}
