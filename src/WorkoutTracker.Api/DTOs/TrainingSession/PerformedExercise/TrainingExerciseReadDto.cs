using WorkoutTracker.Api.DTOs.TrainingSession.Set;

namespace WorkoutTracker.Api.DTOs.Training.Exercise
{
    public record TrainingExerciseReadDto
    {
        public int Id { get; init; }
        public int OrderInSession { get; init; }
        public int ExerciseDefinitionId { get; init; }
        public IList<TrainingSetReadDto> Sets { get; init; } = new List<TrainingSetReadDto>();
    }
}
