using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.Exercise
{
    public record ExerciseDefinitionMuscleGroupLinkDto
    {
        public int ExerciseDefinitionId { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
    }
}
