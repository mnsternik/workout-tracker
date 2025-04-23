namespace WorkoutTracker.Api.Models
{
    public class TrainingSet
    {
        public int Id { get; set; }
        public int TrainingExerciseId { get; set; }
        public int? Reps { get; set; }
        public double? Weight { get; set; }
        public double? Duration { get; set; } // should be mm:ss
        public double? DistanceMeters { get; set; }
    }
}
