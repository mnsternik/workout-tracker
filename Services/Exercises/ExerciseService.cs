﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.DTOs.Exercise;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Utilities;

namespace WorkoutTracker.Api.Services.Exercises
{
    public class ExerciseService : IExerciseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ExerciseService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ExerciseReadDto>> GetExercisesAsync(ExerciseQueryParameters queryParams)
        {
            // Base IQueryable for predefined exercises
            var query = _context.Exercises
                .Include(e => e.MuscleGroupsLinks)
                .AsNoTracking();

            // Filtering and sorting
            query = FilterExercises(query, queryParams);
            query = SortExercises(query, queryParams);

            // Project to DTO
            var dtoQuery = query.ProjectTo<ExerciseReadDto>(_mapper.ConfigurationProvider);

            // Return paginated DTOs list
            return await PaginatedList<ExerciseReadDto>.CreateAsync(dtoQuery, queryParams.PageNumber, queryParams.PageSize);
        }

        public async Task<ExerciseReadDto> GetExerciseAsync(int id)
        {
            var exercise = await _context.Exercises
                .Include(e => e.MuscleGroupsLinks)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
            {
                throw new EntityNotFoundException("Exercise with this ID doesn't exist");
            }

            return _mapper.Map<ExerciseReadDto>(exercise);
        }

        public async Task<ExerciseReadDto> PostExerciseAsync(ExerciseCreateDto exerciseDto)
        {
            var exercise = _mapper.Map<Exercise>(exerciseDto);

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            return _mapper.Map<ExerciseReadDto>(exercise);
        }

        public async Task UpdateExerciseAsync(int id, ExerciseUpdateDto exerciseDto)
        {
            if (id != exerciseDto.Id)
            {
                throw new EntityNotFoundException("ID of an exercise doesn't match with passed ID"); 
            }    

            var exercise = await _context.Exercises
                .Include(e => e.MuscleGroupsLinks)
                .FirstOrDefaultAsync(e => e.Id == exerciseDto.Id);

            if (exercise == null)
            {
                throw new EntityNotFoundException("This exercise doesn't exist");
            }

            _mapper.Map(exerciseDto, exercise);

            exercise.MuscleGroupsLinks.Clear();

            foreach (var muscleGroup in exerciseDto.MuscleGroups)
            {
                exercise.MuscleGroupsLinks.Add(_mapper.Map<ExerciseMuscleGroupLink>(muscleGroup));
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseExists(id))
                {
                    throw new EntityNotFoundException("This exercise was deleted");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteExercise(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                throw new EntityNotFoundException("Exercise with this ID doesn't exist");
            }

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Exercise> FilterExercises(IQueryable<Exercise> query, ExerciseQueryParameters queryParams)
        {
            if (queryParams == null)
            {
                return query;
            }
            if (!string.IsNullOrEmpty(queryParams.Name))
            {
                query = query.Where(e => e.Name.ToLower().Contains(queryParams.Name.ToLower()));
            }
            if (Enum.TryParse<ExerciseType>(queryParams.ExerciseType, ignoreCase: true, out var exerciseTypeEnum))
            {
                query = query.Where(e => e.ExerciseType == exerciseTypeEnum);
            }
            if (Enum.TryParse<MuscleGroup>(queryParams.MuscleGroup, ignoreCase: true, out var muscleGroupEnum))
            {
                query = query.Where(e => e.MuscleGroupsLinks.Any(mgl => mgl.MuscleGroup == muscleGroupEnum));
            }
            if (Enum.TryParse<Equipment>(queryParams.Equipment, ignoreCase: true, out var equipmentEnum))
            {
                query = query.Where(e => e.Equipment == equipmentEnum);
            }
            if (Enum.TryParse<DifficultyLevel>(queryParams.DifficultyLevel, ignoreCase: true, out var difficultyLevelEnum))
            {
                query = query.Where(e => e.DifficultyLevel == difficultyLevelEnum);
            }

            return query;
        }

        private IQueryable<Exercise> SortExercises(IQueryable<Exercise> query, ExerciseQueryParameters queryParams)
        {
            if (!string.IsNullOrEmpty(queryParams.SortBy))
            {
                bool isDescending = queryParams.SortOrder?.ToLower() == "desc";
                switch (queryParams.SortBy)
                {
                    case "name":
                        query = isDescending ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name);
                        break;
                    case "type":
                        query = isDescending ? query.OrderByDescending(t => t.ExerciseType) : query.OrderBy(t => t.ExerciseType);
                        break;
                    case "difficulty":
                        query = isDescending ? query.OrderByDescending(t => t.DifficultyLevel) : query.OrderBy(t => t.DifficultyLevel);
                        break;
                    default:
                        query = isDescending ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name);
                        break;
                }
            }

            return query;
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}
