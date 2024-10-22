using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models.ViewModels
{
    public class SetViewModel
    {
        public int Id { get; set; }

        [Range(1, 99)]
        [Required]
        public int Repetitions { get; set; }

        [Range(0, 999)]
        public int? Weight { get; set; }
    }
}
