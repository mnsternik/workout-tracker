using static WorkoutTracker.Models.Exercise;
using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models.ViewModels
{
    public class ExerciseViewModel
    {
        [StringLength(90, MinimumLength = 2)]
        [Required]
        public string Name { get; set; }

        [StringLength(400)]
        public string? Description { get; set; }

        [Required]
        public ExerciseType Type { get; set; }

        [Required]
        public ICollection<SetViewModel> Sets { get; set; }
    }
}
