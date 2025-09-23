using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.DTOs.ExerciseDefinition;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Utilities;

namespace WorkoutTracker.Api.Services.ExerciseDefinitions
{
    public class ExerciseDefinitionsService : IExerciseDefinitionsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ExerciseDefinitionsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ExerciseDefinitionReadDto>> GetExercisesAsync(ExerciseDefinitionQueryParameters queryParams)
        {
            // Base IQueryable for predefined exercises
            var query = _context.ExerciseDefinitions
                .Include(e => e.MuscleGroupsLinks)
                .AsNoTracking();

            // Filtering and sorting
            query = FilterExercises(query, queryParams);
            query = SortExercises(query, queryParams);

            // Project to DTO
            var dtoQuery = query.ProjectTo<ExerciseDefinitionReadDto>(_mapper.ConfigurationProvider);

            // Return paginated DTOs list
            return await PaginatedList<ExerciseDefinitionReadDto>.CreateAsync(dtoQuery, queryParams.PageNumber, queryParams.PageSize);
        }

        public async Task<ExerciseDefinitionReadDto> GetExerciseAsync(int id)
        {
            var exercise = await _context.ExerciseDefinitions
                .Include(e => e.MuscleGroupsLinks)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null)
            {
                throw new EntityNotFoundException("Exercise with this ID doesn't exist");
            }

            return _mapper.Map<ExerciseDefinitionReadDto>(exercise);
        }

        public async Task<ExerciseDefinitionReadDto> PostExerciseAsync(ExerciseDefinitionCreateDto exerciseDto)
        {
            var exercise = _mapper.Map<ExerciseDefinition>(exerciseDto);

            _context.ExerciseDefinitions.Add(exercise);
            await _context.SaveChangesAsync();

            return _mapper.Map<ExerciseDefinitionReadDto>(exercise);
        }

        public async Task UpdateExerciseAsync(int id, ExerciseDefinitionUpdateDto exerciseDto)
        {
            if (id != exerciseDto.Id)
            {
                throw new EntityNotFoundException("ID of an exercise doesn't match with passed ID"); 
            }    

            var exercise = await _context.ExerciseDefinitions
                .Include(e => e.MuscleGroupsLinks)
                .FirstOrDefaultAsync(e => e.Id == exerciseDto.Id);

            if (exercise == null)
            {
                throw new EntityNotFoundException("Exercise with this ID doesn't exist");
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

        public async Task DeleteExerciseAsync(int id)
        {
            var exercise = await _context.ExerciseDefinitions.FindAsync(id);
            if (exercise == null)
            {
                throw new EntityNotFoundException("Exercise with this ID doesn't exist");
            }

            _context.ExerciseDefinitions.Remove(exercise);
            await _context.SaveChangesAsync();
        }

        private IQueryable<ExerciseDefinition> FilterExercises(IQueryable<ExerciseDefinition> query, ExerciseDefinitionQueryParameters queryParams)
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

        private IQueryable<ExerciseDefinition> SortExercises(IQueryable<ExerciseDefinition> query, ExerciseDefinitionQueryParameters queryParams)
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
            return _context.ExerciseDefinitions.Any(e => e.Id == id);
        }
    }
}
