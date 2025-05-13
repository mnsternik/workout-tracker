
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutTracker.Api.Models
{
    public class TrainingExercise
    {
        public int Id { get; set; }
        public int OrderInSession { get; set; }

        [Required]
        public int TrainingSessionId { get; set; }

        [Required]
        public int ExerciseId { get; set; }
        [ForeignKey(nameof(ExerciseId))]
        public Exercise Exercise { get; set; } = null!;

        public List<TrainingSet> Sets { get; set; } = [];
    }
}
