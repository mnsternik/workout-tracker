namespace WorkoutTracker.Models
{
    public class StrengthExercise : Exercise
    {
        public ICollection<Set> Sets { get; set; }
    }
}
