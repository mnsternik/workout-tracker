using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WorkoutTracker.Api.Controllers;
using WorkoutTracker.Api.DTOs.ExerciseDefinition;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Services.ExerciseDefinitions;
using WorkoutTracker.Api.Utilities;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Controllers
{
    public class ExerciseDefinitionsControllerTests
    {
        private readonly Mock<IExerciseDefinitionsService> _edServiceMock;
        private readonly ExerciseDefinitionsController _edController;

        public ExerciseDefinitionsControllerTests()
        {
            _edServiceMock = new Mock<IExerciseDefinitionsService>();
            _edController = new ExerciseDefinitionsController(_edServiceMock.Object);
        }

        [Fact]
        public async Task GetExercises_ReturnsOk_WithPaginatedList()
        {
            // Arrange
            int pageNumber = 2;
            int pageSize = 10;
            int totalItems = 25;

            var queryParams = new ExerciseDefinitionQueryParameters { PageNumber = pageNumber, PageSize = pageSize };

            var allDtos = new ExerciseDefinitionBuilder().BuildManyReadDtos(totalItems);
            var itemsForPage = allDtos
                .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToList();

            var paginatedList = new PaginatedList<ExerciseDefinitionReadDto>
            (
                itemsForPage,
                totalItems,
                queryParams.PageNumber,
                queryParams.PageSize
            );

            _edServiceMock
                .Setup(s => s.GetExercisesAsync(queryParams))
                .Returns(Task.FromResult(paginatedList));

            // Act
            var actionResult = await _edController.GetExercises(queryParams);

            // Assert
            var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedData = okResult.Value.Should().BeOfType<PaginatedList<ExerciseDefinitionReadDto>>().Subject;

            returnedData.PageIndex.Should().Be(pageNumber);
            returnedData.TotalPages.Should().Be((totalItems + pageSize - 1) / pageSize);
            returnedData.Count.Should().Be(pageSize);
            returnedData.Should().BeEquivalentTo(paginatedList);
        }

        [Fact]
        public async Task GetExercise_ReturnsOk_WithExerciseReadDto()
        {
            // Arrange
            int exerciseId = 1;
            var exerciseDto = new ExerciseDefinitionBuilder().WithId(exerciseId).BuildReadDto();

            _edServiceMock
                .Setup(s => s.GetExerciseAsync(exerciseId))
                .ReturnsAsync(exerciseDto);

            // Act
            var actionResult = await _edController.GetExercise(exerciseId);

            // Assert
            var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(exerciseDto);
        }

        [Fact]
        public async Task GetExercise_ThrowsError_WhenIdNotFound()
        {
            // Arrange
            int notExistingId = 123;
            string expectedErrorMessage = $"Entity '{nameof(ExerciseDefinition)}' with ID '{notExistingId}' not found.";

            _edServiceMock
                .Setup(s => s.GetExerciseAsync(notExistingId))
                .ThrowsAsync(new EntityNotFoundException(nameof(ExerciseDefinition), notExistingId));

            // Act
            Func<Task> act = async () => await _edController.GetExercise(notExistingId);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(expectedErrorMessage);

            _edServiceMock.Verify(s => s.GetExerciseAsync(notExistingId), Times.Once());
        }

        [Fact]
        public async Task PostExercise_ReturnsOk_WithLocation()
        {
            // Arrange
            var exerciseCreateDto = new ExerciseDefinitionBuilder().BuildCreateDto();
            var exerciseReadDto = new ExerciseDefinitionBuilder().BuildReadDto();

            _edServiceMock
                .Setup(s => s.PostExerciseAsync(exerciseCreateDto))
                .ReturnsAsync(exerciseReadDto);

            // Act
            var actionResult = await _edController.PostExercise(exerciseCreateDto);

            // Assert
            var createdAtActionResult = actionResult.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.Value.Should().BeEquivalentTo(exerciseReadDto);

            createdAtActionResult.ActionName.Should().Be(nameof(_edController.GetExercise));
            createdAtActionResult.RouteValues?["id"].Should().Be(exerciseReadDto.Id);
        }

        [Fact]
        public async Task PutExercise_ReturnsNoContent()
        {
            // Arrange
            int updatedExerciseId = 1; 
            var exerciseUpdateDto = new ExerciseDefinitionBuilder().WithId(updatedExerciseId).BuildUpdateDto();

            _edServiceMock
                .Setup(s => s.UpdateExerciseAsync(updatedExerciseId, exerciseUpdateDto))
                .Returns(Task.CompletedTask);

            // Act
            var actionResult = await _edController.PutExercise(updatedExerciseId, exerciseUpdateDto);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
            _edServiceMock.Verify(s => s.UpdateExerciseAsync(updatedExerciseId, exerciseUpdateDto), Times.Once());
        }

        [Fact]
        public async Task PutExercise_ThrowsError_WhenIdNotFound()
        {
            // Arrange
            int notExistingId = 1; 
            var exerciseUpdateDto = new ExerciseDefinitionBuilder().WithId(notExistingId).BuildUpdateDto();
            string expectedErrorMessage = $"Entity '{nameof(ExerciseDefinition)}' with ID '{notExistingId}' not found.";

            _edServiceMock
                .Setup(s => s.UpdateExerciseAsync(notExistingId, exerciseUpdateDto))
                .ThrowsAsync(new EntityNotFoundException(nameof(ExerciseDefinition), notExistingId));

            // Act
            Func<Task> act = async () => await _edController.PutExercise(notExistingId, exerciseUpdateDto);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(expectedErrorMessage);

            _edServiceMock.Verify(s => s.UpdateExerciseAsync(notExistingId, exerciseUpdateDto), Times.Once());
        }

        [Fact]
        public async Task DeleteExercise_ReturnsNoContent()
        {
            // Arrange
            int id = 1;

            _edServiceMock
                .Setup(s => s.DeleteExerciseAsync(id))
                .Returns(Task.CompletedTask);

            // Act
            var actionResult = await _edController.DeleteExercise(id);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
            _edServiceMock.Verify(s => s.DeleteExerciseAsync(id), Times.Once());
        }

        [Fact]
        public async Task DeleteExercise_ThrowsError_WhenIdNotFound()
        {
            // Arrange
            int notExistingId = 1;
            string expectedErrorMessage = $"Entity '{nameof(ExerciseDefinition)}' with ID '{notExistingId}' not found.";

            _edServiceMock
                .Setup(s => s.DeleteExerciseAsync(notExistingId))
                .ThrowsAsync(new EntityNotFoundException(nameof(ExerciseDefinition), notExistingId));

            // Act
            Func<Task> act = async () => await _edController.DeleteExercise(notExistingId);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(expectedErrorMessage);

            _edServiceMock.Verify(s => s.DeleteExerciseAsync(notExistingId), Times.Once());
        }
    }
}
