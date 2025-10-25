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

            // TrainingSessions IDs for asserting
            int fullMatchId = 1;
            int partialMatchId = 2;
            int noMatchId = 3;

            // Defining ExerciseDefinitions
            var fullMatchED = new ExerciseDefinitionBuilder().WithMuscleGroups([..expectedMuscleGroups]).BuildDomain();
            var partialMatchED = new ExerciseDefinitionBuilder().WithMuscleGroups([MuscleGroup.Legs, MuscleGroup.Chest]).BuildDomain();
            var noMatchED = new ExerciseDefinitionBuilder().WithMuscleGroups(MuscleGroup.Legs, MuscleGroup.Abs).BuildDomain();

            // Creating PerformedExercises with ExerciseDefinitions
            var fullMatchPE = new PerformedExerciseBuilder().WithExerciseDefinition(fullMatchED).WithTrainingSessionId(fullMatchId).BuildDomain();
            var partialMatchPE = new PerformedExerciseBuilder().WithExerciseDefinition(partialMatchED).WithTrainingSessionId(partialMatchId).BuildDomain();
            var noMatchPE = new PerformedExerciseBuilder().WithExerciseDefinition(noMatchED).WithTrainingSessionId(noMatchId).BuildDomain();

            // Creating TrainingSessions with full match, partial match and no match filter criteria
            var fullMatchTS = new TrainingSessionBuilder().WithId(fullMatchId).WithPerformedExercises([fullMatchPE]).BuildDomain();
            var partialMatchTS = new TrainingSessionBuilder().WithId(partialMatchId).WithPerformedExercises([partialMatchPE]).BuildDomain();
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
            var result = await TsService.GetTrainingSessionsAsync(queryParams);

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainSingle(ts => ts.Id == fullMatchTS.Id);
        }

        [Fact]
        public async Task GetTrainingSessionsAsync_FiltersByExerciseNames_ReturnsOnlyFullMatches()
        {
            // Arrange
            string matchingName1 = "Push up";
            string matchingName2 = "Running";

            List<string> expectedNames = new List<string> { matchingName1, matchingName2 };
            var queryParams = new TrainingSessionQueryParameters
            {
                PageNumber = 1,
                PageSize = 10,
                ExerciseNames = expectedNames
            };

            // TrainingSessions IDs for asserting
            int fullMatchId = 1;
            int partialMatchID = 2;
            int noMatchID = 3;

            // Defining ExerciseDefinitions
            var pushUp = new ExerciseDefinitionBuilder().WithName(matchingName1).BuildDomain();
            var running = new ExerciseDefinitionBuilder().WithName(matchingName2).BuildDomain();

            // Creating PerformedExercises with ExerciseDefinitions
            var performedPushUp = new PerformedExerciseBuilder().WithExerciseDefinition(pushUp).BuildDomain();
            var performedPushUp2 = new PerformedExerciseBuilder().WithExerciseDefinition(pushUp).BuildDomain();
            var performedRunning = new PerformedExerciseBuilder().WithExerciseDefinition(running).BuildDomain();

            // Creating TrainingSessions with full match, partial match and no match filter criteria
            var fullMatchTs = new TrainingSessionBuilder().WithId(fullMatchId).WithPerformedExercises([performedPushUp, performedRunning]).BuildDomain();
            var partialMatchTs = new TrainingSessionBuilder().WithId(partialMatchID).WithPerformedExercises([performedPushUp2]).BuildDomain();
            var noMatchTs = new TrainingSessionBuilder().WithId(noMatchID).WithDefaultExercises(3).BuildDomain();

            // Seeding database
            Context.TrainingSessions.Add(fullMatchTs);
            Context.TrainingSessions.Add(partialMatchTs);
            Context.TrainingSessions.Add(noMatchTs);
            Context.SaveChanges();

            // Act
            var result = await TsService.GetTrainingSessionsAsync(queryParams);

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainSingle(ts => ts.Id == fullMatchId);
        }

        [Fact]
        public async Task GetTrainingSessionsAsync_FiltersByTrainingName_ReturnsOnlyMatching() // TODO: This does not work boeacuse of Contain(string, StringComprarion)
        {
            // Arrange
            string matchingName = "My Training";
            string notMatchingName = "My Workout";
            var matchingTs = new TrainingSessionBuilder().WithName(matchingName).BuildDomain();
            var notMatchingTs = new TrainingSessionBuilder().WithName(notMatchingName).BuildDomain();

            var queryParams = new TrainingSessionQueryParameters {
                TrainingName = matchingName,
                PageNumber = 1,
                PageSize = 10,
            };

            Context.TrainingSessions.AddRange([matchingTs, notMatchingTs]);
            Context.SaveChanges();

            // Act
            var result = await TsService.GetTrainingSessionsAsync(queryParams);

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainSingle(ts => ts.Name == matchingName);
        }

        [Fact]
        public async Task GetTrainingSessionsAsync_FiltersByUserId_ReturnsOnlyMatching()
        {
            // Arrange
            var matchingUser = new ApplicationUserBuilder().BuildDomain();
            var notMatchingUser = new ApplicationUserBuilder().BuildDomain();

            var matchingTs = new TrainingSessionBuilder().WithUser(matchingUser).BuildDomain();
            var notMatchingTs = new TrainingSessionBuilder().WithUser(notMatchingUser).BuildDomain();

            var queryParams = new TrainingSessionQueryParameters
            {
                UserId = matchingUser.Id,
                PageNumber = 1,
                PageSize = 10,
            };

            Context.TrainingSessions.AddRange([matchingTs, notMatchingTs]);
            Context.SaveChanges();

            // Act
            var result = await TsService.GetTrainingSessionsAsync(queryParams);

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainSingle(ts => ts.UserId == matchingUser.Id);
        }

        [Fact]
        public async Task GetTrainingSessionsAsync_FiltersByUserDisplayedName_ReturnsOnlyMatching()
        {
            // Arrange
            var matchingUser = new ApplicationUserBuilder().WithDisplayName("test1").BuildDomain();
            var notMatchingUser = new ApplicationUserBuilder().WithDisplayName("test2").BuildDomain();

            var matchingTs = new TrainingSessionBuilder().WithUser(matchingUser).BuildDomain();
            var notMatchingTs = new TrainingSessionBuilder().WithUser(notMatchingUser).BuildDomain();

            var queryParams = new TrainingSessionQueryParameters
            {
                UserDisplayName = matchingUser.DisplayName,
                PageNumber = 1,
                PageSize = 10,
            };

            Context.TrainingSessions.AddRange([matchingTs, notMatchingTs]);
            Context.SaveChanges();

            // Act
            var result = await TsService.GetTrainingSessionsAsync(queryParams);

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainSingle(ts => ts.UserId == matchingUser.Id);
        }
    }
}
