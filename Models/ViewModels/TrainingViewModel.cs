using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models.ViewModels
{
    public class TrainingViewModel
    {
        public int? Id { get; set; }

        [StringLength(90, MinimumLength = 2)]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [StringLength(300, MinimumLength = 10)]
        public string? Description { get; set; }

        [Required]
        public ICollection<ExerciseViewModel> Exercises { get; set; }
    }
}
