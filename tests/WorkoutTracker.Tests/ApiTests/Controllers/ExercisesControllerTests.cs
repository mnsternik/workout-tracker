using Moq;
using WorkoutTracker.Api.Controllers;
using WorkoutTracker.Api.DTOs.Exercise;
using WorkoutTracker.Api.Services.Exercises;
using WorkoutTracker.Api.Utilities;
using WorkoutTracker.Tests.SampleData;

namespace WorkoutTracker.Tests.ApiTests.Controllers
{
    public class ExercisesControllerTests
    {
        private readonly Mock<IExerciseService> _mockExerciseService;
        private readonly ExercisesController _exerciseController;

        public ExercisesControllerTests()
        {
            _mockExerciseService = new Mock<IExerciseService>();
            _exerciseController = new ExercisesController(_mockExerciseService.Object);
        }

        [Fact]
        public async Task GetExercises_ReturnsOK_WithExercises()
        {
            // Arrange
            var queryParams = new ExerciseQueryParameters();
            
            //var exercises = ExercisesSampleData.Get();


            //_mockExerciseService.Setup(s => s.GetExercisesAsync(queryParams)).ReturnsAsync(sampleExercises);


            // Act


            // Assert
        }
    }
}
