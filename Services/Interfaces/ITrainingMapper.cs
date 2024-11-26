using WorkoutTracker.Models.ViewModels;
using WorkoutTracker.Models;

namespace WorkoutTracker.Services.Interfaces
{
    public interface ITrainingMapper
    {
        public Training MapToTraining(TrainingViewModel viewModel, string? userId);
        public Training MapToTraining(TrainingViewModel viewModel, Training existingTraining);
        TrainingViewModel MapToViewModel(Training training);
    }
}
