using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public class Set
    {
        public int Id { get; set; }

        [Range(1, 999)]
        [Required]
        public int Repetitions { get; set; }

        [Range(0, 9999)]
        public float? Weight { get; set; } = 0; 

        public int StrengthExerciseId { get; set; }
        public StrengthExercise StrengthExercise { get; set; }
    }
}
