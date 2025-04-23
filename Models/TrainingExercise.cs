
namespace WorkoutTracker.Api.Models
{
    public class TrainingExercise
    {
        public int Id { get; set; }
        public int TrainingSessionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ExerciseType Type { get; set; }

        public List<TrainingSet> Sets { get; set; } = [];
    }
}
