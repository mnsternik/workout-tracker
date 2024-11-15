using System.ComponentModel.DataAnnotations;
using static WorkoutTracker.Models.Exercise;

namespace WorkoutTracker.Models
{
    public class Set : IValidatableObject
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

        public int? Repetitions { get; set; }
        public float? Weight { get; set; }
        public float? Distance { get; set; }
        public float? Duration { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Exercise == null)
            {
                yield return new ValidationResult("Exercise is required.", [nameof(Exercise)]);
                yield break;
            }

            switch (Exercise.Type)
            {
                case ExerciseType.Isometric:
                    if (Duration == null)
                    {
                        yield return new ValidationResult(
                            "Duration is required for isometric exercises.",
                            [nameof(Duration)]
                        );
                    }
                    break;

                case ExerciseType.Strength:
                    if (Repetitions == null)
                    {
                        yield return new ValidationResult(
                            "Repetitions is required for strength exercises.",
                            [nameof(Repetitions)]
                        );
                    }
                    break;

                case ExerciseType.Cardio:
                    if (Distance == null && Duration == null)
                    {
                        yield return new ValidationResult(
                            "Distance or Duration is required for cardio exercises.",
                            [nameof(Distance), nameof(Duration)]
                        );
                    }
                    break;
            }
        }
    }
}
