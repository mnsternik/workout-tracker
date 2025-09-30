using FluentAssertions;
using WorkoutTracker.Api.DTOs.ExerciseDefinition;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Utilities;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class GetExercisesAsyncTests : ExerciseDefinitionsServiceTestsBase
    {
        [Fact]
        public async Task GetExercisesAsync_ReturnsPaginatedList()
        {
            // Arrange
            var pageNumber = 2;
            var pageSize = 10;
            var queryParams = new ExerciseDefinitionQueryParameters { PageNumber = pageNumber, PageSize = pageSize };
            SeedDatabaseWithDefaults();

            // Act
            var exercises = await Service.GetExercisesAsync(queryParams);

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

            // Seeding database with 1 object with target value and another with other value
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithDifficultyLevel(expectedDifficultyLevel).BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithDifficultyLevel(DifficultyLevel.Intermediate).BuildDomain());
            Context.SaveChanges();

            // Act
            var exercises = await Service.GetExercisesAsync(queryParams);

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

            // Seeding database with 1 object with target value and another with other value
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithExerciseType(expectedExerciseType).BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithExerciseType(ExerciseType.Strength).BuildDomain());
            Context.SaveChanges();

            // Act
            var exercises = await Service.GetExercisesAsync(queryParams);

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

            // Seeding database with 1 object with target value and another with other value
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithName(nameFilter).BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithName("Other name").BuildDomain());
            Context.SaveChanges();

            // Act
            var exercises = await Service.GetExercisesAsync(queryParams);

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

            // Seeding database with 1 object with target value and another with other value
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithEquipment(expectedEquipment).BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithEquipment(Equipment.Bench).BuildDomain());
            Context.SaveChanges();

            // Act
            var exercises = await Service.GetExercisesAsync(queryParams);

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

            // Seeding database with 1 object with target value and another with other value
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithMuscleGroups(expectedMuscleGroups).BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithMuscleGroups(MuscleGroup.Abs).BuildDomain());
            Context.SaveChanges();

            // Act
            var exercises = await Service.GetExercisesAsync(queryParams);

            // Assert
            var exercise = exercises.Should().ContainSingle().Subject;
            exercise.MuscleGroups.Should().Contain(expectedMuscleGroups);
        }

        [Fact]
        public async Task GetExercisesAsync_FiltersByMuscleGroup_ShouldReturnEmptyList_WhenNoFullMatch()
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
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithMuscleGroups(MuscleGroup.Triceps).BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithMuscleGroups(MuscleGroup.Abs).BuildDomain());
            Context.SaveChanges();

            // Act
            var exercises = await Service.GetExercisesAsync(queryParams);

            // Assert
            exercises.Should().BeEmpty();
        }

        [Fact]
        public async Task GetExercisesAsync_SortsByName_Ascending()
        {
            // Arrange
            var queryParams = new ExerciseDefinitionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                SortOrder = "asc",
                SortBy = "name"
            };

            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithName("Push up").BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithName("Bench press").BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithName("Dead bug").BuildDomain());
            Context.SaveChanges();

            // Act
            var exercises = await Service.GetExercisesAsync(queryParams);

            // Assert
            exercises.Should().HaveCountGreaterThan(1);
            exercises.Should().BeInAscendingOrder(e => e.Name);
        }

        [Fact]
        public async Task GetExercisesAsync_SortsByName_Descending()
        {
            // Arrange
            var queryParams = new ExerciseDefinitionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                SortOrder = "desc",
                SortBy = "name"
            };

            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithName("Push up").BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithName("Bench press").BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithName("Dead bug").BuildDomain());
            Context.SaveChanges();

            // Act
            var exercises = await Service.GetExercisesAsync(queryParams);

            // Assert
            exercises.Should().HaveCountGreaterThan(1);
            exercises.Should().BeInDescendingOrder(e => e.DifficultyLevel);
        }

        [Fact]
        public async Task GetExercisesAsync_SortsByDifficultyLevel_Ascending()
        {
            // Arrange
            var queryParams = new ExerciseDefinitionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                SortOrder = "asc",
                SortBy = "difficulty"
            };

            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithDifficultyLevel(DifficultyLevel.Intermediate).BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithDifficultyLevel(DifficultyLevel.Beginner).BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithDifficultyLevel(DifficultyLevel.Advanced).BuildDomain());
            Context.SaveChanges();

            // Act
            var exercises = await Service.GetExercisesAsync(queryParams);

            // Assert

            exercises.Should().HaveCount(3);
            exercises.Should().BeInAscendingOrder(e => (int)e.DifficultyLevel);
        }

        [Fact]
        public async Task GetExercisesAsync_SortsByDifficultyLevel_Descending()
        {
            // Arrange
            var queryParams = new ExerciseDefinitionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                SortOrder = "desc",
                SortBy = "difficulty"
            };

            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithDifficultyLevel(DifficultyLevel.Intermediate).BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithDifficultyLevel(DifficultyLevel.Beginner).BuildDomain());
            Context.ExerciseDefinitions.Add(new ExerciseDefinitionBuilder().WithDifficultyLevel(DifficultyLevel.Advanced).BuildDomain());
            Context.SaveChanges();

            // Act
            var exercises = await Service.GetExercisesAsync(queryParams);

            // Assert
            exercises.Should().HaveCount(3);
            exercises.Should().BeInDescendingOrder(e => (int)e.DifficultyLevel);
        }
    }
}
