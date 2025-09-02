namespace WorkoutTracker.Api.Models
{
    public class PerformedSet
    {
        public int Id { get; set; }
        public int OrderInExercise { get; set; }
        public int? Reps { get; set; }
        public decimal? WeightKg { get; set; }
        public int? DurationSeconds { get; set; }
        public int? DistanceMeters { get; set; }
        public int PerformedExerciseId { get; set; }
        public PerformedExercise PerformedExercise { get; set; } = null!; 
    }
}
