using Moq;
using WorkoutTracker.Api.Controllers;
using WorkoutTracker.Api.Services.ExerciseDefinitions;

namespace WorkoutTracker.Tests.ApiTests.Controllers
{
    public class ExercisesControllerTests
    {
        private readonly Mock<IExerciseDefinitionsService> _mockExerciseService;
        private readonly ExerciseDefinitionsController _exerciseController;

        public ExercisesControllerTests()
        {
            _mockExerciseService = new Mock<IExerciseDefinitionsService>();
            _exerciseController = new ExerciseDefinitionsController(_mockExerciseService.Object);
        }


        [Fact]
        public async Task GetExercises_ReturnsOK_WithExercises()
        {
            throw new NotImplementedException();
        }
    }
}
