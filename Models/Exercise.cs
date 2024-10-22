using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public abstract class Exercise
    {
        public int Id { get; set; }

        [StringLength(90, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [StringLength(300, MinimumLength = 10)]
        [Required]
        public string Description { get; set; }
    }
}
