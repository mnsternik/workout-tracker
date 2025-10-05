using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Api.DTOs.PerformedSet
{
    public record PerformedSetUpdateDto
    {
        public int Id { get; init; }

        [Range(0, 999)]
        public int OrderInExercise { get; init; }

        [Range(1, 999)]
        public int? Reps { get; init; }

        [Range(0, 9999.99)]
        public decimal? WeightKg { get; init; }

        [Range(1, 86400)]
        public int? DurationSeconds { get; init; }

        [Range(1, 1000000)]
        public int? DistanceMeters { get; init; }
    }
}
