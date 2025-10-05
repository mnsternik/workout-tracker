using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WorkoutTracker.Api.Controllers;
using WorkoutTracker.Api.DTOs.TrainingSession;
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

        [Fact]
        public async Task GetTrainingSessions_ShouldReturnOk_WithPaginatedList()
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
        }


    }
}
