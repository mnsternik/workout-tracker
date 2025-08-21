namespace WorkoutTracker.Api.Models
{
    public class ExerciseMuscleGroupLink
    {
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;
        public MuscleGroup MuscleGroup { get; set; }
    }
}
