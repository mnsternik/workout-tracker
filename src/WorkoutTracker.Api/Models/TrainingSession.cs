namespace WorkoutTracker.Api.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DifficultyRating? DifficultyRating { get; set; }
        public int? DurationMinutes { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public ICollection<PerformedExercise> PerformedExercises { get; set; } = [];
    }

    public enum DifficultyRating
    {
        VeryEasy,
        Easy,
        Medium, 
        Hard,
        Extreme
    }
}
