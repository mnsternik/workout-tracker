namespace WorkoutTracker.Api.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? DifficultyRating { get; set; }
        public int? EstimatedDurationTimeMinutes { get; set; }

        public List<TrainingExercise> Exercises { get; set; } = [];

    }
}
