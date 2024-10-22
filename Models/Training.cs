using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public class Training
    {
        public int Id { get; set; }

        [StringLength(90, MinimumLength = 3)]
        public string? Name { get; set; }

        [StringLength(300, MinimumLength = 10)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; } 

        public ICollection<Exercise> Exercises { get; set; }
    }
}
