using System.ComponentModel.DataAnnotations;
using WorkoutTracker.Api.DTOs.TrainingSession.Set;

namespace WorkoutTracker.Api.DTOs.Training.Exercise
{
    public record TrainingExerciseCreateDto
    {
        [Range(0, int.MaxValue)]
        public int OrderInSession { get; init; }

        [Required]
        public int ExerciseDefinitionId { get; init; }

        [Required]
        public IList<TrainingSetCreateDto> Sets { get; init; } = new List<TrainingSetCreateDto>();
    }
}
