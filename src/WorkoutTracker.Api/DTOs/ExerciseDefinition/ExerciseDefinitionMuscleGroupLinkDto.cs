using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.ExerciseDefinition
{
    public record ExerciseDefinitionMuscleGroupLinkDto
    {
        public int ExerciseDefinitionId { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
    }
}
