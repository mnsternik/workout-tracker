using FluentAssertions;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class UpdateExerciseAsyncTests : ExerciseDefinitionsServiceTestsBase
    {
        [Fact]
        public async Task UpdateExerciseAsync_SuccessfullyUpdatesName()
        {
            // Arrange
            string oldName = "Push up";
            string updatedName = "Diamond push up";
            int exerciseId = 1;
            var exerciseToUpdate = new ExerciseDefinitionBuilder().WithId(exerciseId).WithName(oldName).BuildDomain();

            Context.ExerciseDefinitions.Add(exerciseToUpdate);
            Context.SaveChanges();

            // Act
            var updatedExerciseDto = new ExerciseDefinitionBuilder().WithId(exerciseId).WithName(updatedName).BuildUpdateDto();
            await Service.UpdateExerciseAsync(exerciseId, updatedExerciseDto);

            // Assert
            var fetchedExericse = Context.ExerciseDefinitions.Find(exerciseId);
            fetchedExericse.Should().NotBeNull();
            fetchedExericse.Name.Should().Be(updatedName);
        }

        [Fact]
        public async Task UpdateExerciseAsync_ThrowsError_WhenIdNotMatches()
        {
            // Arrange
            int id = 1;
            int differentId = 99;
            var exerciseDto = new ExerciseDefinitionBuilder().WithId(differentId).BuildUpdateDto();
            string expectedErrorMessage = $"ID of an exercise '{exerciseDto.Id}' doesn't match passed ID '{id}'";

            // Act
            Func<Task> act = async () => await Service.UpdateExerciseAsync(id, exerciseDto);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(expectedErrorMessage);
        }

        [Fact]
        public async Task UpdateExerciseAsync_ThrowsError_WhenExerciseNotFound()
        {
            // Arrange
            int notExistingId = 127;
            string expectedErrorMessage = $"Entity '{nameof(ExerciseDefinition)}' with ID '{notExistingId}' not found.";
            var exerciseDto = new ExerciseDefinitionBuilder().WithId(notExistingId).BuildUpdateDto();

            // Act
            Func<Task> act = async () => await Service.UpdateExerciseAsync(notExistingId, exerciseDto);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(expectedErrorMessage);
        }
    }
}
