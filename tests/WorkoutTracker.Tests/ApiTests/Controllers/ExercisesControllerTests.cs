using Moq;
using WorkoutTracker.Api.Controllers;
using WorkoutTracker.Api.Services.Exercises;

namespace WorkoutTracker.Tests.ApiTests.Controllers
{
    public class ExercisesControllerTests
    {
        private readonly Mock<IExerciseService> _mockExerciseService;
        private readonly ExerciseDefinitionsController _exerciseController;

        public ExercisesControllerTests()
        {
            _mockExerciseService = new Mock<IExerciseService>();
            _exerciseController = new ExerciseDefinitionsController(_mockExerciseService.Object);
        }


        [Fact]
        public async Task GetExercises_ReturnsOK_WithExercises()
        {
            throw new NotImplementedException();
        }
    }
}
