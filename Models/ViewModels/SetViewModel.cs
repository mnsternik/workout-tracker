using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models.ViewModels
{
    public class SetViewModel
    {
        public int Id { get; set; }

        [Range(1, 999)]
        [Required]
        public int Repetitions { get; set; }

        [Range(0, 9999)]
        public float? Weight { get; set; } = 0;
    }
}
