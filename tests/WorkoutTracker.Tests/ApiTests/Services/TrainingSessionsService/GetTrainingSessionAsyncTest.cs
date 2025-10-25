using FluentAssertions;
using WorkoutTracker.Api.DTOs.TrainingSession;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class GetTrainingSessionAsyncTest : TrainingSessionsServiceTestsBase
    {
        [Fact]
        public async Task GetTrainingSession_ReturnsReadDto_WhenSuccesss()
        {
            // Arrange
            int id = 1;
            var ts = new TrainingSessionBuilder().WithId(id).BuildDomain();

            Context.TrainingSessions.Add(ts);
            Context.SaveChanges();

            // Act
            var result = await TsService.GetTrainingSessionAsync(id);

            // Assert
            result.Should().BeOfType<TrainingSessionReadDto>();
            result.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetTrainingSession_ThrowsError_WhenEntityNotFound()
        {
            // Arrange
            int notExistingId = 999;
            string errorMessage = $"Entity '{nameof(TrainingSession)}' with ID '{notExistingId}' not found.";

            // Act 
            Func<Task> act = async () => await TsService.GetTrainingSessionAsync(notExistingId);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(errorMessage);
        }
    }
}
