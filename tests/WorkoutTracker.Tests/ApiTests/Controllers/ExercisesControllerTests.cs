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

<<<<<<< HEAD
        [Fact]
        public async Task GetExercises_ReturnsOK_WithExercises()
        {
            // Arrange
=======
        //[Fact]
        //public async Task GetExercises_ReturnsOK_WithExercises()
        //{
        //    // Arrange
        //    var queryParams = new ExerciseQueryParameters();
            
        //    //var exercises = ExercisesSampleData.Get();


        //    //_mockExerciseService.Setup(s => s.GetExercisesAsync(queryParams)).ReturnsAsync(sampleExercises);
>>>>>>> 9818cb74ac0ead0c921eb8b06a6864f53e47d8af


        //    // Act


        //    // Assert
        //}
    }
}
