using FluentAssertions;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class DeleteExerciseAsync : ExerciseDefinitionsServiceTestsBase
    {
        [Fact]
        public async Task DeleteExerciseAsync_ThrowsError_WhenEntityNotFound()
        {
            // Arrange
            int notExistingId = 71;
            string errorMessage = "Exercise with this ID doesn't exist";

            // Act
            Func<Task> act = async () => await Service.DeleteExerciseAsync(notExistingId);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(errorMessage);
        }

        [Fact]
        public async Task DeleteExerciseAsync_SuccessfullyDeletesExercise()
        {
            // Arrange
            int exerciseId = 1;
            var exercise = new ExerciseDefinitionBuilder().WithId(exerciseId).BuildDomain();

            Context.Add(exercise);
            Context.SaveChanges();

            // Act
            await Service.DeleteExerciseAsync(exerciseId);

            // Assert
            var deletedExercise = Context.ExerciseDefinitions.Find(exerciseId);
            deletedExercise.Should().BeNull();
        }
    }
}
