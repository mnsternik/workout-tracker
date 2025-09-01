namespace WorkoutTracker.Api.Models
{
    public class ExerciseMuscleGroupLink
    {
        public int ExerciseDefinitionId { get; set; }
        public ExerciseDefinition ExerciseDefinition { get; set; } = null!;
        public MuscleGroup MuscleGroup { get; set; }
    }
}
