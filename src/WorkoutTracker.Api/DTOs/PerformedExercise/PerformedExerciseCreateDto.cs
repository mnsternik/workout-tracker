using System.ComponentModel.DataAnnotations;
using WorkoutTracker.Api.DTOs.PerformedSet;

namespace WorkoutTracker.Api.DTOs.PerformedExercise
{
    public record PerformedExerciseCreateDto
    {
        [Range(0, int.MaxValue)]
        public int OrderInSession { get; init; }

        [Required]
        public int ExerciseDefinitionId { get; init; }

        [Required]
        public IList<PerformedSetCreateDto> Sets { get; init; } = new List<PerformedSetCreateDto>();
    }
}
