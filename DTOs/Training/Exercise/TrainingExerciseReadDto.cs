using WorkoutTracker.Api.DTOs.Training.Set;

namespace WorkoutTracker.Api.DTOs.Training.Exercise
{
    public record TrainingExerciseReadDto
    {
        public int Id { get; init; }
        public int OrderInSession { get; init; }
        public int ExerciseId { get; init; }
        public IList<TrainingSetReadDto> Sets { get; init; } = new List<TrainingSetReadDto>();
    }
}
