using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.DTOs.ExerciseDefinition;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Mapping;
using WorkoutTracker.Api.Services.ExerciseDefinitions;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class GetExerciseAsyncTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExerciseDefinitionsService _service;

        public GetExerciseAsyncTests()
        {
            // In-memory database configuration
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            // AutoMapper configuration 
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();

            _service = new ExerciseDefinitionsService(_context, _mapper);
        }

        private void SeedDatabaseWithDefaults()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.ExerciseDefinitions.AddRange(new ExerciseDefinitionBuilder().BuildManyDomains(25));
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetExerciseAsync_ReturnsExerciseDto()
        {
            // Arrange
            int id = 10;
            SeedDatabaseWithDefaults();

            // Act 
            var exercise = await _service.GetExerciseAsync(id);

            // Assert
            exercise.Should().BeOfType<ExerciseDefinitionReadDto>();
            exercise.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetExerciseAsync_ThrowsError_WhenEntityNotFound()
        {
            // Arrange
            int notExistingId = 999;
            string errorMessage = "Exercise with this ID doesn't exist";
            SeedDatabaseWithDefaults();

            // Act 
            Func<Task> act = async () => await _service.GetExerciseAsync(notExistingId);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(errorMessage);
        }
    }
}
