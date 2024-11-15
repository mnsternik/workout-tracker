using System.ComponentModel.DataAnnotations;
using static WorkoutTracker.Models.Exercise;

namespace WorkoutTracker.Models.ViewModels
{
    public class SetViewModel : IValidatableObject
    {
        public int? Repetitions { get; set; }
        public float? Weight { get; set; }
        public float? Distance { get; set; }
        public float? Duration { get; set; }
        
        [Required]
        public ExerciseType ExerciseType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (ExerciseType)
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
