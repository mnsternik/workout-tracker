using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using WorkoutTracker.Api.Controllers;
using WorkoutTracker.Api.DTOs.TrainingSession;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Services.TrainingSessions;
using WorkoutTracker.Api.Utilities;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Controllers
{
    public class TrainingSessionsControllerTests
    {
        private readonly Mock<ITrainingSessionsService> _tsServiceMock;
        private readonly TrainingSessionsController _tsController;

        public TrainingSessionsControllerTests()
        {
            _tsServiceMock = new Mock<ITrainingSessionsService>();
            _tsController = new TrainingSessionsController(_tsServiceMock.Object);
        }

        private string SimulateUser()
        {
            string userId = Guid.NewGuid().ToString();
            var userClaims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            };

            var userIdentity = new ClaimsIdentity(userClaims);
            var userPrincipals = new ClaimsPrincipal(userIdentity);

            _tsController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipals }
            };

            return userId;
        }

        [Fact]
        public async Task GetTrainingSessions_ReturnsOk_WithPaginatedList()
        {
            // Arrange
            int pageNumber = 2;
            int pageSize = 10;
            int totalItems = 25; 

            var queryParams = new TrainingSessionQueryParameters { PageNumber = pageNumber, PageSize = pageSize };

            var allDtos = new TrainingSessionBuilder().BuildManyReadDtos(totalItems);
            var itemsForPage = allDtos
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            var paginatedList = new PaginatedList<TrainingSessionReadDto>(
                itemsForPage,
                totalItems,
                queryParams.PageNumber,
                queryParams.PageSize);

            _tsServiceMock
                .Setup(s => s.GetTrainingSessionsAsync(queryParams))
                .ReturnsAsync(paginatedList);

            // Act
            var actionResult = await _tsController.GetTrainingSessions(queryParams);

            // Assert
            var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedData = okResult.Value.Should().BeOfType<PaginatedList<TrainingSessionReadDto>>().Subject;

            returnedData.PageIndex.Should().Be(pageNumber);
            returnedData.TotalPages.Should().Be((totalItems + pageSize - 1) / pageSize);
            returnedData.Should().HaveCount(pageSize);
            returnedData.Should().BeEquivalentTo(paginatedList);
        }

        [Fact]
        public async Task GetTrainingSession_ReturnsOk_WithTrainingReadDto()
        {
            // Arrange
            int exerciseId = 1; 
            var exercise = new TrainingSessionBuilder().WithId(exerciseId).BuildReadDto();

            _tsServiceMock
                .Setup(s => s.GetTrainingSessionAsync(exerciseId))
                .ReturnsAsync(exercise);

            // Act
            var actionResult = await _tsController.GetTrainingSession(exerciseId);

            // Assert
            var okResult = actionResult.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(exercise);

            _tsServiceMock.Verify(s => s.GetTrainingSessionAsync(exerciseId), Times.Once());
        }

        [Fact]
        public async Task GetTrainingSession_ThrowsError_WhenIdNotFound()
        {
            // Arrange
            int id = 1;
            string expectedErrorMessage = $"Entity 'TrainingSession' with ID '{id}' not found.";

            _tsServiceMock
                .Setup(s => s.GetTrainingSessionAsync(id))
                .ThrowsAsync(new EntityNotFoundException(nameof(TrainingSession), id));

            // Act
            Func<Task> act = async () => await _tsController.GetTrainingSession(id);

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage(expectedErrorMessage);

            _tsServiceMock.Verify(s => s.GetTrainingSessionAsync(id), Times.Once());
        }

        [Fact]
        public async Task PostTrainingSession_ReturnsCreatedAtAction_WithLocation()
        {
            // Arrange
            string userId = SimulateUser();
            var tsCreateDto = new TrainingSessionBuilder().BuildCreateDto();
            var tsReadDto = new TrainingSessionBuilder().BuildReadDto();

            _tsServiceMock
                .Setup(s => s.PostTrainingSessionAsync(userId, tsCreateDto))
                .ReturnsAsync(tsReadDto);

            // Act
            var actionResult = await _tsController.PostTrainingSession(tsCreateDto);

            // Assert
            var createdAtActionResult = actionResult.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.Value.Should().BeEquivalentTo(tsReadDto);

            createdAtActionResult.ActionName.Should().Be(nameof(_tsController.GetTrainingSession));
            createdAtActionResult.RouteValues?["id"].Should().Be(tsReadDto.Id);
        }

        [Fact]
        public async Task PutTrainingSession_ReturnsNoContent()
        {
            // Arrange
            int tsId = 1;
            string userId = SimulateUser();
            var tsUpdateDto = new TrainingSessionBuilder().WithId(tsId).BuildUpdateDto();

            _tsServiceMock
                .Setup(s => s.UpdateTrainingSessionAsync(tsId, userId, tsUpdateDto))
                .Returns(Task.CompletedTask);

            // Act
            var actionResult = await _tsController.PutTrainingSession(tsId, tsUpdateDto);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
            _tsServiceMock.Verify(s => s.UpdateTrainingSessionAsync(tsId, userId, tsUpdateDto), Times.Once);
        }

        [Fact]
        public async Task DeleteTrainingSession_ReturnsNoContent()
        {
            // Arrange
            string userId = SimulateUser();
            int tsId = 1;

            _tsServiceMock
                .Setup(s => s.DeleteTrainingSession(tsId, userId))
                .Returns(Task.CompletedTask);

            // Act
            var actionResult = await _tsController.DeleteTrainingSession(tsId);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
            _tsServiceMock.Verify(s => s.DeleteTrainingSession(tsId, userId), Times.Once);
        }
    }
}
