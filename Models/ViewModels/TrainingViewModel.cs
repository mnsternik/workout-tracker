using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models.ViewModels
{
    public class TrainingViewModel
    {
        [StringLength(90, MinimumLength = 2)]
        [Required]
        public string Name { get; set; }


        [StringLength(300, MinimumLength = 10)]
        public string? Description { get; set; }

        [Required]
        public ICollection<ExerciseViewModel> Exercises { get; set; }
    }
}
