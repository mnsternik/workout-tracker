using WorkoutTracker.Api.DTOs.Training.TrainingSession;
using WorkoutTracker.Api.DTOs.TrainingSession.TrainingSession;
using WorkoutTracker.Api.Utilities;

namespace WorkoutTracker.Api.Services.TrainingSessions
{
    public interface ITrainingSessionsService
    {
        Task<PaginatedList<TrainingSessionReadDto>> GetTrainingSessionsAsync(TrainingSessionQueryParameters queryParams);
        Task<TrainingSessionReadDto?> GetTrainingSessionAsync(int id);
        Task UpdateTrainingSessionAsync(int sessionId, string? currentUserId, TrainingSessionUpdateDto trainingSessionDto);
        Task<TrainingSessionReadDto> PostTrainingSessionAsync(string? currentUserId, TrainingSessionCreateDto trainingSession);
        Task DeleteTrainingSession(int sessionId, string? currentUserId);
    }
}
