using WorkoutTracker.Api.DTOs.Training.Exercise;
using WorkoutTracker.Api.DTOs.Training.TrainingSession;
using WorkoutTracker.Api.DTOs.TrainingSession.TrainingSession;
using WorkoutTracker.Api.Utilities;

namespace WorkoutTracker.Api.Services.TrainingSessions
{
    public interface ITrainingSessionsService
    {
        Task<PaginatedList<TrainingSessionReadDto>> GetTrainingSessionsAsync(TrainingSessionQueryParameters queryParams);
    }
}
