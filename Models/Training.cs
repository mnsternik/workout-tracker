using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutTracker.Models
{
    public class Training
    {
        public int Id { get; set; }

        [StringLength(90, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [StringLength(300, MinimumLength = 10)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }

        public ICollection<Exercise> Exercises { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }  
}
