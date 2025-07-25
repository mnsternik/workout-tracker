﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.DTOs.Training.TrainingSession;
using WorkoutTracker.Api.DTOs.TrainingSession.TrainingSession;
using WorkoutTracker.Api.Exceptions;
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

        public async Task<TrainingSessionReadDto?> GetTrainingSessionAsync(int id)
        {
            var trainingSession = await _context.TrainingSessions
                .Include(t => t.User)
                .Include(t => t.Exercises)
                    .ThenInclude(te => te.Exercise)
                        .ThenInclude(e => e.MuscleGroupsLinks)
                .Include(t => t.Exercises)
                    .ThenInclude(te => te.Sets)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainingSession == null)
            {
                throw new EntityNotFoundException($"Training session with Id {id} not found");
            }

            return _mapper.Map<TrainingSessionReadDto>(trainingSession);
        }

        public async Task UpdateTrainingSessionAsync(int id, string? currentUserId, TrainingSessionUpdateDto trainingSessionDto)
        {
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new UnauthorizedActionException("User ID not found in token");
            }

            var sessionToUpdate = await _context.TrainingSessions
                .Include(t => t.Exercises)
                    .ThenInclude(te => te.Sets)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (sessionToUpdate == null)
            {
                throw new EntityNotFoundException($"Training session with Id {id} not found");
            }
            if (currentUserId != sessionToUpdate.UserId)
            {
                throw new UnauthorizedActionException("You are not authorized to update another user training session.");
            }

            // Update basic properties
            sessionToUpdate.Name = trainingSessionDto.Name;
            sessionToUpdate.Notes = trainingSessionDto.Notes;
            sessionToUpdate.DurationMinutes = trainingSessionDto.EstimatedDurationMinutes;
            sessionToUpdate.DifficultyRating = trainingSessionDto.DifficultyRating;

            // Clear existing exercises and sets
            _context.TrainingSets.RemoveRange(sessionToUpdate.Exercises.SelectMany(e => e.Sets));
            _context.TrainingExercises.RemoveRange(sessionToUpdate.Exercises);
            sessionToUpdate.Exercises.Clear();

            // Add new exercises and their sets
            foreach (var exerciseDto in trainingSessionDto.Exercises)
            {
                var exercise = _mapper.Map<TrainingExercise>(exerciseDto);
                exercise.TrainingSessionId = id;

                sessionToUpdate.Exercises.Add(exercise);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingSessionExists(id))
                {
                    throw new EntityNotFoundException($"Training session with Id {id} was deleted");
                }
                else
                {
                    throw new DbUpdateConcurrencyException($"The training session with Id {id} was updated by another process. Please reload and try again.");
                }
            }
        }

        public async Task<TrainingSessionReadDto> PostTrainingSessionAsync(string? currentUserId, TrainingSessionCreateDto trainingSessionDto)
        {
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new UnauthorizedActionException("User ID not found in token");
            }

            var trainingSession = _mapper.Map<TrainingSession>(trainingSessionDto);
            trainingSession.UserId = currentUserId;

            _context.TrainingSessions.Add(trainingSession);
            await _context.SaveChangesAsync();

            // Return created entity's DTO
            return _mapper.Map<TrainingSessionReadDto>(trainingSession);
        }

        public async Task DeleteTrainingSession(int sessionId, string? currentUserId)
        {
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new UnauthorizedActionException("User ID not found in token");
            }

            var trainingSession = await _context.TrainingSessions.FindAsync(sessionId);

            if (trainingSession == null)
            {
                throw new EntityNotFoundException($"Training session with Id {sessionId} not found");
            }
            if (trainingSession.UserId != currentUserId)
            {
                throw new UnauthorizedActionException("You are not authorized to delete this training session.");
            }

            _context.TrainingSessions.Remove(trainingSession);
            await _context.SaveChangesAsync();
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

        private bool TrainingSessionExists(int id)
        {
            return _context.TrainingSessions.Any(e => e.Id == id);
        }
    }
}
