using WorkoutTracker.Models.ViewModels;
using WorkoutTracker.Models;
using WorkoutTracker.Services.Interfaces;

namespace WorkoutTracker.Services.Mappers
{
    public class TrainingMapper : ITrainingMapper
    {
        public TrainingViewModel MapToViewModel(Training training)
        {
            return new TrainingViewModel
            {
                Name = training.Name,
                Description = training.Description,
                Date = training.Date,
                Exercises = training.Exercises.Select(e => new ExerciseViewModel
                {
                    Name = e.Name,
                    Description = e.Description,
                    Type = e.Type,
                    Sets = e.Sets.Select(s => new SetViewModel
                    {
                        ExerciseType = e.Type,
                        Repetitions = s.Repetitions,
                        Weight = s.Weight,
                        Distance = s.Distance,
                        Duration = s.Duration,
                    }).ToList()
                }).ToList()
            };
        }

        public Training MapToTraining(TrainingViewModel viewModel, string? userId)
        {
            return new Training
            {
                UserId = userId,
                Date = DateTime.UtcNow,
                Name = viewModel.Name,
                Description = viewModel.Description,
                Exercises = MapExercises(viewModel.Exercises)
            };
        }

        public Training MapToTraining(TrainingViewModel viewModel, Training existingTraining)
        {
            existingTraining.Name = viewModel.Name;
            existingTraining.Description = viewModel.Description;
            existingTraining.Exercises = MapExercises(viewModel.Exercises);
            return existingTraining;
        }

        private List<Exercise> MapExercises(IEnumerable<ExerciseViewModel> exercises)
        {
            return exercises.Select(e => new Exercise
            {
                Name = e.Name,
                Description = e.Description,
                Type = e.Type,
                Sets = e.Sets.Select(s => new Set
                {
                    Repetitions = s.Repetitions,
                    Weight = s.Weight,
                    Distance = s.Distance,
                    Duration = s.Duration
                }).ToList()
            }).ToList();
        }
    }
}
