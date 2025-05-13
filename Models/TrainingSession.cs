using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Api.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? DifficultyRating { get; set; }
        public int? EstimatedDurationMinutes { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public ICollection<TrainingExercise> Exercises { get; set; } = [];

    }
}
