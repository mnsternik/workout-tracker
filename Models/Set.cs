using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public class Set
    {
        public int Id { get; set; }

        [Range(1, 99)]
        [Required]
        public int Repetitions { get; set; }

        [Range(0, 999)]
        public int? Weight { get; set; }

        public int StrengthExerciseId { get; set; }
        public StrengthExercise StrengthExercise { get; set; }
    }
}
