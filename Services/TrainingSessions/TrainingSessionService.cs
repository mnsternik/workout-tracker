using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.DTOs.Training.TrainingSession;
using WorkoutTracker.Api.DTOs.TrainingSession.TrainingSession;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Utilities;

namespace WorkoutTracker.Api.Services.TrainingSessions
{
    public class TrainingSessionService : ITrainingSessionsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TrainingSessionService(ApplicationDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TrainingSessionReadDto>> GetTrainingSessionsAsync(TrainingSessionQueryParameters queryParams)
        {
            // Base IQueryable for training sessions
            var query = _context.TrainingSessions
                .Include(t => t.User)
                .Include(t => t.Exercises)
                    .ThenInclude(te => te.Exercise)
                        .ThenInclude(e => e.MuscleGroupsLinks)
                .Include(t => t.Exercises)
                    .ThenInclude(te => te.Sets)
                .AsNoTracking();

            // Sorting and filtering
            query = FilterTrainingSessions(query, queryParams);
            query = SortTrainingSessions(query, queryParams);

            // Project to DTO
            var dtoQuery = query.ProjectTo<TrainingSessionReadDto>(_mapper.ConfigurationProvider);

            // Return paginated DTOs list
            return await PaginatedList<TrainingSessionReadDto>.CreateAsync(dtoQuery, queryParams.PageNumber, queryParams.PageSize);
        }

        private IQueryable<TrainingSession> FilterTrainingSessions(IQueryable<TrainingSession> query, TrainingSessionQueryParameters queryParams)
        {
            if (!string.IsNullOrEmpty(queryParams.UserId))
            {
                query = query.Where(t => t.UserId == queryParams.UserId);
            }
            if (!string.IsNullOrEmpty(queryParams.DisplayName))
            {
                query = query.Where(t => t.User.DisplayName.ToLower().Contains(queryParams.DisplayName.ToLower()));
            }
            if (queryParams.MinDifficulty.HasValue)
            {
                query = query.Where(t => t.DifficultyRating >= queryParams.MinDifficulty.Value);
            }
            if (queryParams.MaxDifficulty.HasValue)
            {
                query = query.Where(t => t.DifficultyRating <= queryParams.MaxDifficulty.Value);
            }
            if (queryParams.MinDurationMinutes.HasValue)
            {
                query = query.Where(t => t.DurationMinutes >= queryParams.MinDurationMinutes.Value);
            }
            if (queryParams.MaxDurationMinutes.HasValue)
            {
                query = query.Where(t => t.DurationMinutes <= queryParams.MaxDurationMinutes.Value);
            }
            if (queryParams.ExerciseNames != null && queryParams.ExerciseNames.Count > 0)
            {
                var lowerExerciseNames = queryParams.ExerciseNames.Select(e => e.ToLower()).ToList();
                query = query.Where(t => t.Exercises.Any(te => lowerExerciseNames.Contains(te.Exercise.Name.ToLower())));
            }
            if (queryParams.MuscleGroups != null && queryParams.MuscleGroups.Count > 0)
            {
                var lowerMuscleGroups = queryParams.MuscleGroups.Select(mg => mg.ToLower()).ToList();
                query = query.Where(t => t.Exercises.Any(te =>
                    te.Exercise.MuscleGroupsLinks.Any(mgl =>
                        lowerMuscleGroups.Contains(mgl.MuscleGroup.ToString().ToLower()))));
            }

            return query; 
        }

        private IQueryable<TrainingSession> SortTrainingSessions(IQueryable<TrainingSession> query, TrainingSessionQueryParameters queryParams)
        {
            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                bool isDescending = queryParams.SortOrder?.ToLower() == "desc";
                switch (queryParams.SortBy)
                {
                    case "name":
                        query = isDescending ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name);
                        break;
                    case "createdat":
                        query = isDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt);
                        break;
                    case "duration":
                        query = isDescending ? query.OrderByDescending(t => t.DurationMinutes) : query.OrderBy(t => t.DurationMinutes);
                        break;
                    case "difficulty":
                        query = isDescending ? query.OrderByDescending(t => t.DifficultyRating) : query.OrderBy(t => t.DifficultyRating);
                        break;
                    default:
                        query = isDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt);
                        break;
                }
            }

            return query; 
        }
    }
}
