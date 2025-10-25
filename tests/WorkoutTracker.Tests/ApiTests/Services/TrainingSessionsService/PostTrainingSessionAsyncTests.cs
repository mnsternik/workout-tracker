using FluentAssertions;
using WorkoutTracker.Api.DTOs.TrainingSession;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class PostTrainingSessionAsyncTests : TrainingSessionsServiceTestsBase
    {
        [Fact]
        public async Task PostTrainingSession_ReturnsReadDto_WhenSuccess()
        {
            // Arrange
            var tsCreateDto = new TrainingSessionBuilder().WithDefaultExercises(0).BuildCreateDto();
            var user = new ApplicationUserBuilder().BuildDomain();
            Context.Users.Add(user);

            // Act
            var result = await TsService.PostTrainingSessionAsync(user.Id, tsCreateDto);

            // Assert
            result.Should().BeOfType<TrainingSessionReadDto>();
            result.Id.Should().NotBe(null);
            result.UserId.Should().Be(user.Id);
        }

        [Fact]
        public async Task PostTrainingSession_ThrowsError_WhenUserIdIsNull()
        {
            // Arrange
            var tsCreateDto = new TrainingSessionBuilder().WithDefaultExercises(0).BuildCreateDto();
            string errorMessage = "User ID not found in token";

            // Act
            Func<Task> act = async () => await TsService.PostTrainingSessionAsync(null, tsCreateDto);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedActionException>()
                .WithMessage(errorMessage);
        }
    }
}
