using FluentAssertions;
using WorkoutTracker.Api.DTOs.TrainingSession;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Utilities;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class GetTrainingSessionsAsyncTests : TrainingSessionsServiceTestsBase
    {
        [Fact]
        public async Task GetTrainingSessionsAsyncTests_ReturnsPaginatedList()
        {
            // Arrange
            int pageNumber = 2;
            int pageSize = 10;
            int totalItems = 25;
            var queryParams = new TrainingSessionQueryParameters() { PageNumber = pageNumber, PageSize = pageSize };
            SeedWithDefaultSessions(totalItems);

            // Act
            var trainingSessions = await TsService.GetTrainingSessionsAsync(queryParams);

            // Assert
            trainingSessions.Should().BeOfType<PaginatedList<TrainingSessionReadDto>>();
            trainingSessions.Should().HaveCount(pageSize);
            trainingSessions.PageIndex.Should().Be(pageNumber);
        }

        [Fact]
        public async Task GetTrainingSessionsAsync_FiltersByDifficultyRating_ReturnsOnlyMatching()
        {
            // Arrange
            var expectedDifficultyRating = DifficultyRating.Medium;
            var queryParams = new TrainingSessionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                DifficultyRating = expectedDifficultyRating.ToString()
            };

            // Seeding database with 1 object with target value and another with other value
            Context.TrainingSessions.Add(new TrainingSessionBuilder().WithDifficultyRating(expectedDifficultyRating).BuildDomain());
            Context.TrainingSessions.Add(new TrainingSessionBuilder().WithDifficultyRating(DifficultyRating.Extreme).BuildDomain());
            Context.SaveChanges();

            // Act
            var trainingSessions = await TsService.GetTrainingSessionsAsync(queryParams);

            // Assert
            trainingSessions.Should().HaveCount(1);
            trainingSessions.Select(e => e.DifficultyRating).Should().OnlyContain(dr => dr == expectedDifficultyRating);
        }

        [Fact]
        public async Task GetTrainingSessionsAsync_FiltersByMinDurationMinutes_ReturnsOnlyMatching()
        {
            // Arrange
            int minDurationMinutes = 45;
            var queryParams = new TrainingSessionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                MinDurationMinutes = minDurationMinutes
            };

            // Seeding database with 1 object with target value and another with other value
            Context.TrainingSessions.Add(new TrainingSessionBuilder().WithDurationMinutes(minDurationMinutes + 5).BuildDomain());
            Context.TrainingSessions.Add(new TrainingSessionBuilder().WithDurationMinutes(minDurationMinutes - 5).BuildDomain());
            Context.SaveChanges();

            // Act
            var trainingSessions = await TsService.GetTrainingSessionsAsync(queryParams);

            // Assert
            trainingSessions.Should().HaveCount(1);
            trainingSessions.Select(e => e.DurationMinutes).Should().OnlyContain(dl => dl > minDurationMinutes);
        }

        [Fact]
        public async Task GetTrainingSessionsAsync_FiltersByMaxDurationMinutes_ReturnsOnlyMatching()
        {
            // Arrange
            int maxDurationMinutes = 45;
            var queryParams = new TrainingSessionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                MaxDurationMinutes = maxDurationMinutes
            };

            // Seeding database with 1 object with target value and another with other value
            Context.TrainingSessions.Add(new TrainingSessionBuilder().WithDurationMinutes(maxDurationMinutes - 5).BuildDomain());
            Context.TrainingSessions.Add(new TrainingSessionBuilder().WithDurationMinutes(maxDurationMinutes + 5).BuildDomain());
            Context.SaveChanges();

            // Act
            var trainingSessions = await TsService.GetTrainingSessionsAsync(queryParams);

            // Assert
            trainingSessions.Should().HaveCount(1);
            trainingSessions.Select(e => e.DurationMinutes).Should().OnlyContain(dl => dl < maxDurationMinutes);
        }

        [Fact]
        public async Task GetTrainingSessionsAsync_FiltersByMuscleGroups_ReturnsOnlyFullMatches()
        {
            // Arrange
            List<MuscleGroup> expectedMuscleGroups = [MuscleGroup.Triceps, MuscleGroup.Chest];
            var queryParams = new TrainingSessionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                MuscleGroups = expectedMuscleGroups.Select(emg => emg.ToString()).ToList()
            };

            // Building TrainingSession with PerformedExercise that containts expected MuscleGroups
            int fullMatchId = 1;
            var fullMatchED = new ExerciseDefinitionBuilder().WithId(fullMatchId).WithMuscleGroups([..expectedMuscleGroups]).BuildDomain();
            var fullMatchPE = new PerformedExerciseBuilder().WithExerciseDefinition(fullMatchED).WithTrainingSessionId(fullMatchId).BuildDomain();
            var fullMatchTS = new TrainingSessionBuilder().WithId(fullMatchId).WithPerformedExercises([fullMatchPE]).BuildDomain();

            // Building TrainingSession with PerformedExercise that containts only part of excpected MuscleGroups
            int partialMatchId = 2;
            var partialMatchED = new ExerciseDefinitionBuilder().WithId(partialMatchId).WithMuscleGroups([MuscleGroup.Legs, MuscleGroup.Chest]).BuildDomain();
            var partialMatchPE = new PerformedExerciseBuilder().WithExerciseDefinition(partialMatchED).WithTrainingSessionId(partialMatchId).BuildDomain();
            var partialMatchTS = new TrainingSessionBuilder().WithId(partialMatchId).WithPerformedExercises([partialMatchPE]).BuildDomain();

            // Building TrainingSession with PerformedExercise that does NOT containt expected MuscleGroups
            int noMatchId = 3;
            var noMatchED = new ExerciseDefinitionBuilder().WithId(noMatchId).WithMuscleGroups(MuscleGroup.Legs, MuscleGroup.Abs).BuildDomain();
            var noMatchPE = new PerformedExerciseBuilder().WithExerciseDefinition(noMatchED).WithTrainingSessionId(noMatchId).BuildDomain();
            var noMatchTS = new TrainingSessionBuilder().WithId(noMatchId).WithPerformedExercises([noMatchPE]).BuildDomain();

            // Seeding database 
            Context.ExerciseDefinitions.Add(fullMatchED);
            Context.TrainingSessions.Add(fullMatchTS);
            Context.ExerciseDefinitions.Add(partialMatchED);
            Context.TrainingSessions.Add(partialMatchTS);
            Context.ExerciseDefinitions.Add(noMatchED);
            Context.TrainingSessions.Add(noMatchTS);
            Context.SaveChanges();

            // Act
            var trainingSessions = await TsService.GetTrainingSessionsAsync(queryParams);

            // Assert
            trainingSessions.Should().ContainSingle(ts => ts.Id == fullMatchTS.Id);
        }
    }
}
