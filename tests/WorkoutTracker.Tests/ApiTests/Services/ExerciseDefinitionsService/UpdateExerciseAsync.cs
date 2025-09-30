using FluentAssertions;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class UpdateExerciseAsync : ExerciseDefinitionsServiceTestsBase
    {
        [Fact]
        public async Task UpdateExerciseAsync_ThrowsError_WhenIdNotMatches()
        {
            // Arrange
            var exerciseId = 1;
            var exerciseDto = new ExerciseDefinitionBuilder().WithId(exerciseId).BuildUpdateDto();
            var errorMessage = "ID of an exercise doesn't match with passed ID";

            // Act
            exerciseDto.Id = 5;
            Func<Task> act = async () => await Service.UpdateExerciseAsync(exerciseId, exerciseDto);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(errorMessage);
        }

        [Fact]
        public async Task UpdateExerciseAsync_ThrowsError_WhenExerciseNotFound()
        {
            // Arrange
            var notExistingId = 127; 
            var exerciseDto = new ExerciseDefinitionBuilder().WithId(notExistingId).BuildUpdateDto();
            var errorMessage = "Exercise with this ID doesn't exist";

            // Act
            Func<Task> act = async () => await Service.UpdateExerciseAsync(notExistingId, exerciseDto);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(errorMessage);
        }
    }
}
