using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.Exercise
{
    public record ExerciseMuscleGroupLinkDto
    {
        public int ExerciseId { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
    }
}
