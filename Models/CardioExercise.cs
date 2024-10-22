using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public class CardioExercise : Exercise
    {
        [Required]
        public TimeSpan Duration { get; set; }
        public float? Distance { get; set; }
        public int? AvarageBMP { get; set; }
    }
}
