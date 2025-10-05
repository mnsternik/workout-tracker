using FluentAssertions;
using WorkoutTracker.Api.DTOs.ExerciseDefinition;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class GetExerciseAsyncTests : ExerciseDefinitionsServiceTestsBase
    {
        [Fact]
        public async Task GetExerciseAsync_ReturnsExerciseDto()
        {
            // Arrange
            var id = 10;
            var exerciseToRetrive = new ExerciseDefinitionBuilder().WithId(id).BuildDomain();
            Context.ExerciseDefinitions.Add(exerciseToRetrive);
            Context.SaveChanges();

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
            int notExistingId = 9999;
            string errorMessage = "Exercise with this ID doesn't exist";

            // Act 
            Func<Task> act = async () => await Service.GetExerciseAsync(notExistingId);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(errorMessage);
        }
    }
}
