namespace WorkoutTracker.Api.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? DifficultyRating { get; set; }
        public int? EstimatedDurationMinutes { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public ICollection<TrainingExercise> Exercises { get; set; } = [];
    }
}
