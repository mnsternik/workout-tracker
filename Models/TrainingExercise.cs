
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutTracker.Api.Models
{
    public class TrainingExercise
    {
        public int Id { get; set; }
        public int TrainingSessionId { get; set; }
        public int ExerciseId { get; set; }

        public List<TrainingSet> Sets { get; set; } = [];

        [ForeignKey(nameof(ExerciseId))]
        public Exercise Exercise { get; set; } 
    }
}
