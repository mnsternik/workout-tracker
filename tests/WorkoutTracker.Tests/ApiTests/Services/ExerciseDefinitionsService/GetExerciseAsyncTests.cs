using FluentAssertions;
using WorkoutTracker.Api.DTOs.ExerciseDefinition;
using WorkoutTracker.Api.Exceptions;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class GetExerciseAsyncTests : ExerciseDefinitionsServiceTestsBase
    {
        [Fact]
        public async Task GetExerciseAsync_ReturnsExerciseDto()
        {
            // Arrange
            int id = 10;
            SeedDatabaseWithDefaults();

            // Act 
            var exercise = await Service.GetExerciseAsync(id);

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
            Func<Task> act = async () => await Service.GetExerciseAsync(notExistingId);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(errorMessage);
        }
    }
}
