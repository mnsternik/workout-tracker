using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.DTOs.Exercise;
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

        //public async Task<PaginatedList<ExerciseReadDto>> GetExercisesAsync()
        //{
        //    var exerciesQuery = _context.Exercises
        //        .Include(e => e.MuscleGroupsLinks);

            // TODO: Add filtering exercises
            // TODO: Add sorting exercises

            // TODO: Moved here all logic from controller
        //}
    }
}
