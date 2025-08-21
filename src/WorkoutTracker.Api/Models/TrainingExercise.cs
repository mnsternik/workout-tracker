namespace WorkoutTracker.Api.Models
{
    public class TrainingExercise
    {
        public int Id { get; set; }
        public int OrderInSession { get; set; }
        public int TrainingSessionId { get; set; }
        public TrainingSession TrainingSession { get; set; } = null!;
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;
        public ICollection<TrainingSet> Sets { get; set; } = [];
    }
}
