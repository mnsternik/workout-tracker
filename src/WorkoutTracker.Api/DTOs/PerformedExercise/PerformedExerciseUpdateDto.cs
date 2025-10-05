using System.ComponentModel.DataAnnotations;
using WorkoutTracker.Api.DTOs.PerformedSet;

namespace WorkoutTracker.Api.DTOs.PerformedExercise
{
    public record PerformedExerciseUpdateDto
    {
        public int Id { get; init; }

        [Range(0, int.MaxValue)]
        public int OrderInSession { get; init; }

        [Required]
        public int ExerciseDefinitionId { get; init; }

        [Required]
        public IList<PerformedSetUpdateDto> Sets { get; init; } = [];
    }
}
