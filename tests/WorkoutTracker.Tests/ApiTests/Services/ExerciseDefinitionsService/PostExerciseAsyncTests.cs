using FluentAssertions;
using WorkoutTracker.Api.DTOs.ExerciseDefinition;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class PostExerciseAsyncTests : ExerciseDefinitionsServiceTestsBase
    {
        [Fact]
        public async Task PostExerciseAsync_ReturnsReadDto_WhenSuccess()
        {
            // Arrange
            var exerciseDto = new ExerciseDefinitionBuilder().BuildCreateDto();

            // Act
            var result = await EdService.PostExerciseAsync(exerciseDto);

            // Assert
            result.Should().BeOfType<ExerciseDefinitionReadDto>();
            result.Id.Should().NotBe(null);
        }
    }
}
