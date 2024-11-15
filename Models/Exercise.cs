using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public class Exercise
    {
        public enum ExerciseType
        {
            Strength,
            Cardio,
            Isometric
        }

        public int Id { get; set; }

        [StringLength(90, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [StringLength(400)]
        public string? Description { get; set; }

        [Required]
        public ExerciseType Type { get; set; }

        public ICollection<Set> Sets { get; set; }
    }
}
