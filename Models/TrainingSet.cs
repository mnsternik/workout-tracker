using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutTracker.Api.Models
{
    public class TrainingSet
    {
        public int Id { get; set; }
        public int OrderInExercise { get; set; }
        public int? Reps { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal? WeightKg { get; set; }
        public int? DurationSeconds { get; set; }
        public int? DistanceMeters { get; set; }

        [Required]
        public int TrainingExerciseId { get; set; }
    }
}
