using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.Training
{
    public class CreateTrainingSessionDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int? DifficultyRating { get; set; }
        public List<TrainingExercise> Exercises { get; set; } = [];
    }
}
