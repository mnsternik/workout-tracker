using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models.ViewModels
{
    public class TrainingViewModel
    {
        public int Id { get; set; }

        [StringLength(90, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(300, MinimumLength = 10)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }

        public List<ExerciseViewModel> Exercises { get; set; } = new List<ExerciseViewModel>();

        public TrainingViewModel()
        {
            Date = DateTime.Now;
        }
    }
}
