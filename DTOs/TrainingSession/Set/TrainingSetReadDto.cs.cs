namespace WorkoutTracker.Api.DTOs.Training.Set
{
    public record TrainingSetReadDto
    {
        public int Id { get; init; }
        public int OrderInExercise { get; init; }
        public int? Reps { get; init; }
        public decimal? WeightKg { get; init; }
        public int? DurationSeconds { get; init; }
        public int? DistanceMeters { get; init; }
    }
}
