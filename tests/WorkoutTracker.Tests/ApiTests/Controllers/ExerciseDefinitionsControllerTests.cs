using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WorkoutTracker.Api.Controllers;
using WorkoutTracker.Api.DTOs.ExerciseDefinition;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Services.ExerciseDefinitions;
using WorkoutTracker.Api.Utilities;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Controllers
{
    public class ExerciseDefinitionsControllerTests
    {
        private readonly Mock<IExerciseDefinitionsService> _mockExerciseService;
        private readonly ExerciseDefinitionsController _exerciseController;

        public ExerciseDefinitionsControllerTests()
        {
            _mockExerciseService = new Mock<IExerciseDefinitionsService>();
            _exerciseController = new ExerciseDefinitionsController(_mockExerciseService.Object);
        }

        [Fact]
        public async Task GetExercises_ReturnsOk_WithExercises()
        {
            // Arrange
            int pageNumber = 2;
            int pageSize = 10;

            var queryParams = new ExerciseDefinitionQueryParameters { PageNumber = pageNumber, PageSize = pageSize };
            var allDtos = new ExerciseDefinitionBuilder().BuildManyReadDtos(25);

            var itemsForPage = allDtos
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToList();

            var serviceResponseData = new PaginatedList<ExerciseDefinitionReadDto>
            (
                itemsForPage,
                allDtos.Count,
                queryParams.PageNumber,
                queryParams.PageSize
            );

            _mockExerciseService
                .Setup(s => s.GetExercisesAsync(queryParams))
                .Returns(Task.FromResult(serviceResponseData));

            // Act
            var actionResult = await _exerciseController.GetExercises(queryParams);

            // Assert
            var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedData = okResult.Value.Should().BeOfType<PaginatedList<ExerciseDefinitionReadDto>>().Subject;

            returnedData.PageIndex.Should().Be(pageNumber);
            returnedData.TotalPages.Should().Be(3);
            returnedData.Count.Should().Be(10);
        }

        [Fact]
        public async Task GetExercise_ReturnsOk_WithExercise()
        {
            // Arrange
            int exerciseId = 1;
            var exerciseDto = new ExerciseDefinitionBuilder().WithId(exerciseId).BuildReadDto();

            _mockExerciseService
                .Setup(s => s.GetExerciseAsync(exerciseId))
                .ReturnsAsync(exerciseDto);


            // Act
            var actionResult = await _exerciseController.GetExercise(exerciseId);

            // Assert
            var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedData = okResult.Value.Should().BeOfType<ExerciseDefinitionReadDto>().Subject;

            returnedData.Id.Should().Be(exerciseId); 
        }

        [Fact]
        public async Task GetExercise_ThrowsError_WhenIdNotFound()
        {
            // Arrange
            int notExistingId = 123;
            string errorMessage = "Exercise with this ID doesn't exist";

            _mockExerciseService
                .Setup(s => s.GetExerciseAsync(notExistingId))
                .ThrowsAsync(new EntityNotFoundException(errorMessage));

            // Act
            Func<Task> act = async () => await _exerciseController.GetExercise(notExistingId);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(errorMessage);

            _mockExerciseService.Verify(s => s.GetExerciseAsync(notExistingId), Times.Once());
        }

        [Fact]
        public async Task PostExercise_ReturnsOk_WithLocation()
        {
            // Arrange
            var exerciseCreateDto = new ExerciseDefinitionBuilder().BuildCreateDto();
            var exerciseReadDto = new ExerciseDefinitionBuilder().BuildReadDto();

            _mockExerciseService
                .Setup(s => s.PostExerciseAsync(exerciseCreateDto))
                .ReturnsAsync(exerciseReadDto);

            // Act
            var actionResult = await _exerciseController.PostExercise(exerciseCreateDto);

            // Assert
            var createdAtActionResult = actionResult.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.Value.Should().BeEquivalentTo(exerciseReadDto);

            createdAtActionResult.ActionName.Should().Be(nameof(_exerciseController.GetExercise));
            createdAtActionResult.RouteValues?["id"].Should().Be(exerciseReadDto.Id);
        }

        [Fact]
        public async Task PutExercise_ReturnsNoContent()
        {
            // Arrange
            var updatedExerciseId = 1; 
            var exerciseUpdateDto = new ExerciseDefinitionBuilder().WithId(updatedExerciseId).BuildUpdateDto();

            _mockExerciseService
                .Setup(s => s.UpdateExerciseAsync(updatedExerciseId, exerciseUpdateDto))
                .Returns(Task.CompletedTask);

            // Act
            var actionResult = await _exerciseController.PutExercise(updatedExerciseId, exerciseUpdateDto);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
            _mockExerciseService.Verify(s => s.UpdateExerciseAsync(updatedExerciseId, exerciseUpdateDto), Times.Once());
        }

        [Fact]
        public async Task PutExercise_ThrowsError_WhenIdNotFound()
        {
            // Arrange
            var notExistingId = 1; 
            var exerciseUpdateDto = new ExerciseDefinitionBuilder().WithId(notExistingId).BuildUpdateDto();
            string errorMessage = "Exercise with this ID doesn't exist";

            _mockExerciseService
                .Setup(s => s.UpdateExerciseAsync(notExistingId, exerciseUpdateDto))
                .ThrowsAsync(new EntityNotFoundException(errorMessage));

            // Act
            Func<Task> act = async () => await _exerciseController.PutExercise(notExistingId, exerciseUpdateDto);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(errorMessage);

            _mockExerciseService.Verify(s => s.UpdateExerciseAsync(notExistingId, exerciseUpdateDto), Times.Once());
        }

        [Fact]
        public async Task DeleteExercise_ReturnsNoContent_WhenSuccess()
        {
            // Arrange
            int id = 1;

            _mockExerciseService
                .Setup(s => s.DeleteExerciseAsync(id))
                .Returns(Task.CompletedTask);

            // Act
            var actionResult = await _exerciseController.DeleteExercise(id);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
            _mockExerciseService.Verify(s => s.DeleteExerciseAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteExercise_ThrowsError_WhenIdNotFound()
        {
            // Arrange
            int notExistingId = 1;

            _mockExerciseService
                .Setup(s => s.DeleteExerciseAsync(notExistingId))
                .ThrowsAsync(new EntityAlreadyExistsException("Exercise with this ID doesn't exist"));

            // Act
            Func<Task> act = async () => await _exerciseController.DeleteExercise(notExistingId);

            // Assert
            await act.Should().ThrowAsync<EntityAlreadyExistsException>()
                .WithMessage("Exercise with this ID doesn't exist");

            _mockExerciseService.Verify(s => s.DeleteExerciseAsync(notExistingId), Times.Once());
        }

    }
}
