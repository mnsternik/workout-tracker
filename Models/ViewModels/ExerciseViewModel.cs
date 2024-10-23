using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models.ViewModels
{
    public class ExerciseViewModel
    {
        public int Id { get; set; }

        [StringLength(90, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }

        public List<SetViewModel> Sets { get; set; } = new List<SetViewModel>();
    }
}
