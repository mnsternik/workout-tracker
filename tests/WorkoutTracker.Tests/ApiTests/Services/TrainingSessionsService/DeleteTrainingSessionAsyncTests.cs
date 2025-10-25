using FluentAssertions;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class DeleteTrainingSessionAsyncTests : TrainingSessionsServiceTestsBase
    {
        [Fact]
        public async Task DeleteTrainingSessionAsync_ThrowsError_WhenEntityNotFound()
        {
            // Arrange
            int notExistingId = 999;
            string userId = Guid.NewGuid().ToString();
            string errorMessage = $"Entity '{nameof(TrainingSession)}' with ID '{notExistingId}' not found.";

            // Act
            Func<Task> act = async () => await TsService.DeleteTrainingSession(notExistingId, userId);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(errorMessage);
        }

        [Fact]
        public async Task DeleteTrainingSessionAsync_SuccessfullyDeletesSession()
        {
            // Arrange
            int sessionId = 1;
            var user = new ApplicationUserBuilder().BuildDomain();
            var session = new TrainingSessionBuilder().WithId(sessionId).WithUser(user).BuildDomain();

            Context.TrainingSessions.Add(session);
            Context.SaveChanges();

            // Act
            await TsService.DeleteTrainingSession(sessionId, user.Id);

            // Assert
            var result = Context.TrainingSessions.Find(sessionId);
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteTrainingSessionAsync_ThrowsError_WhenUserIsNotAuthor()
        {
            // Arrange
            int sessionId = 1;
            string randomUserId = Guid.NewGuid().ToString();
            string errorMessage = "User not authorized to delete this training session.";

            var user = new ApplicationUserBuilder().BuildDomain();
            var session = new TrainingSessionBuilder().WithId(sessionId).WithUser(user).BuildDomain();

            Context.TrainingSessions.Add(session);
            Context.SaveChanges();

            // Act
            Func<Task> act = async () => await TsService.DeleteTrainingSession(sessionId, randomUserId);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedActionException>()
                .WithMessage(errorMessage);
        }
    }
}
