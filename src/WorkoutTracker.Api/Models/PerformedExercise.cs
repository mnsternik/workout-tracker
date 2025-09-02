namespace WorkoutTracker.Api.Models
{
    public class PerformedExercise
    {
        public int Id { get; set; }
        public int OrderInSession { get; set; }
        public int TrainingSessionId { get; set; }
        public TrainingSession TrainingSession { get; set; } = null!;
        public int ExerciseDefinitionId { get; set; }
        public ExerciseDefinition ExerciseDefinition { get; set; } = null!;
        public ICollection<PerformedSet> Sets { get; set; } = [];
    }
}
