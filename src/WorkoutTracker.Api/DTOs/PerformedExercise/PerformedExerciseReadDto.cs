using WorkoutTracker.Api.DTOs.PerformedSet;

namespace WorkoutTracker.Api.DTOs.PerformedExercise
{
    public record PerformedExerciseReadDto
    {
        public int Id { get; init; }
        public int OrderInSession { get; init; }
        public int ExerciseDefinitionId { get; init; }
        public IList<PerformedSetReadDto> Sets { get; init; } = new List<PerformedSetReadDto>();
    }
}
