using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.DTOs.ExerciseDefinition;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Mapping;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Services.ExerciseDefinitions;
using WorkoutTracker.Api.Utilities;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class ExerciseDefinitionsServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper; 
        private readonly IExerciseDefinitionsService _service;
            
        public ExerciseDefinitionsServiceTests() 
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
        public async Task GetExercisesAsync_ReturnsPaginatedList()
        {
            // Arrange

            var pageNumber = 2;
            var pageSize = 10;
            var queryParams = new ExerciseDefinitionQueryParameters { PageNumber = pageNumber, PageSize = pageSize };

            SeedDatabaseWithDefaults();

            // Act
            var exercises = await _service.GetExercisesAsync(queryParams);

            // Assert
            exercises.Should().BeOfType<PaginatedList<ExerciseDefinitionReadDto>>();
            exercises.Count.Should().BeGreaterThan(0);
            exercises.PageIndex.Should().Be(pageNumber);
        }

        [Fact]
        public async Task GetExercisesAsync_FiltersByDifficultyLevel_ReturnsOnlyMatching()
        {
            // Arrange
            var expectedDifficultyLevel = DifficultyLevel.Advanced;
            var queryParams = new ExerciseDefinitionQueryParameters 
            { 
                PageNumber = 1,
                PageSize = 10,
                DifficultyLevel = expectedDifficultyLevel.ToString() 
            };

            // Seeding database with 1 object with target value and one with other value
            _context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithDifficultyLevel(expectedDifficultyLevel).BuildDomain());
            _context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithDifficultyLevel(DifficultyLevel.Intermediate).BuildDomain());
            _context.SaveChanges();

            // Act
            var exercises = await _service.GetExercisesAsync(queryParams);

            // Assert
            exercises.Should().HaveCount(1);
            exercises.Select(e => e.DifficultyLevel).Should().OnlyContain(dl => dl == expectedDifficultyLevel);
        }

        [Fact]
        public async Task GetExercisesAsync_FiltersByExerciseType_ReturnsOnlyMatching()
        {
            // Arrange
            var expectedExerciseType = ExerciseType.Cardio;
            var queryParams = new ExerciseDefinitionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                ExerciseType = expectedExerciseType.ToString()
            };

            // Seeding database with 1 object with target value and one with other value
            _context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithExerciseType(expectedExerciseType).BuildDomain());
            _context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithExerciseType(ExerciseType.Strength).BuildDomain());
            _context.SaveChanges();

            // Act
            var exercises = await _service.GetExercisesAsync(queryParams);

            // Assert
            exercises.Should().HaveCount(1);
            exercises.Select(e => e.ExerciseType).Should().OnlyContain(et => et == expectedExerciseType);
        }

        [Fact]
        public async Task GetExercisesAsync_FiltersByName_ReturnsOnlyMatching()
        {
            // Arrange
            var nameFilter = "somename";
            var queryParams = new ExerciseDefinitionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                Name = nameFilter
            };

            // Seeding database with 1 object with target value and one with other value
            _context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithName(nameFilter).BuildDomain());
            _context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithName("Other name").BuildDomain());
            _context.SaveChanges();

            // Act
            var exercises = await _service.GetExercisesAsync(queryParams);

            // Assert
            exercises.Should().HaveCount(1);
            exercises.Select(e => e.Name).Should().OnlyContain(n => n.Contains(nameFilter));
        }

        [Fact]
        public async Task GetExercisesAsync_FiltersByEquipment_ReturnsOnlyMatching()
        {
            // Arrange
            var expectedEquipment = Equipment.PullUpBar;
            var queryParams = new ExerciseDefinitionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                Equipment = expectedEquipment.ToString()
            };

            // Seeding database with 1 object with target value and one with other value
            _context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithEquipment(expectedEquipment).BuildDomain());
            _context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithEquipment(Equipment.Bench).BuildDomain());
            _context.SaveChanges();

            // Act
            var exercises = await _service.GetExercisesAsync(queryParams);

            // Assert
            exercises.Should().HaveCount(1);
            exercises.Select(e => e.Equipment).Should().OnlyContain(e => e == expectedEquipment);
        }

        [Fact]
        public async Task GetExercisesAsync_FiltersByMuscleGroup_ReturnsOnlyMatching()
        {
            // Arrange
            MuscleGroup[] expectedMuscleGroups = { MuscleGroup.Triceps, MuscleGroup.Chest };
            var queryParams = new ExerciseDefinitionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                MuscleGroups = expectedMuscleGroups.Select(mg => mg.ToString()).ToArray()
            };

            // Seeding database with 1 object with target value and one with other value
            _context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithMuscleGroups(expectedMuscleGroups).BuildDomain());
            _context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithMuscleGroups(MuscleGroup.Abs).BuildDomain());
            _context.SaveChanges();

            // Act
            var exercises = await _service.GetExercisesAsync(queryParams);

            // Assert
            var exercise = exercises.Should().ContainSingle().Subject;
            exercise.MuscleGroups.Select(mg => mg.MuscleGroup).Should().Contain(expectedMuscleGroups);
        }

        [Fact]
        public async Task GetExercisesAsync_FiltersByMuscleGroup_ShouldReturnEmptyList_WhenNotFullMatch()
        {
            // Arrange
            MuscleGroup[] selectedMuscleGroups = { MuscleGroup.Triceps, MuscleGroup.Chest };
            var queryParams = new ExerciseDefinitionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                MuscleGroups = selectedMuscleGroups.Select(smg => smg.ToString()).ToArray()
            };

            // Seeding database with a partial matching object and not-at-all matching object 
            _context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithMuscleGroups(MuscleGroup.Triceps).BuildDomain());
            _context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithMuscleGroups(MuscleGroup.Abs).BuildDomain());
            _context.SaveChanges();

            // Act
            var exercises = await _service.GetExercisesAsync(queryParams);

            // Assert
            exercises.Should().BeEmpty();
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
