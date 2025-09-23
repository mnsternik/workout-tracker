using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Tests.Builders
{
    public class TrainingSessionBuilder
    {
        private int _id = 1;
        private string _name = "My training session";
        private string? _note = "Notes to my trainig session";
        private DateTime _createdAt = DateTime.UtcNow;
        private DifficultyRating? _difficultyRating = DifficultyRating.Easy;
        private int? _durationMinutes = 45;
        private string _userId = new Guid().ToString();
        private ApplicationUser _user = null!;
        private ICollection<PerformedExercise> _performedExercises = [];

        // Declares how many generated PerformedExercises with default values each TrainingSession will have 
        private int _exercisesCount = 1; 

        public TrainingSessionBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public TrainingSessionBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public TrainingSessionBuilder WithNote(string note)
        {
            _note = note;
            return this;
        }

        public TrainingSessionBuilder WithCreateDate(DateTime date)
        {
            _createdAt = date;
            return this;
        }

        public TrainingSessionBuilder WithDifficultyRating(DifficultyRating difficulty)
        {
            _difficultyRating = difficulty;
            return this;
        }

        public TrainingSessionBuilder WithDurationMinutes(int duration)
        {
            _durationMinutes = duration;
            return this;
        }

        public TrainingSessionBuilder WithUserId(string userId)
        {
            _userId = userId;
            return this;
        }

        public TrainingSessionBuilder WithUser(ApplicationUser user)
        {
            _user = user;
            _userId = user.Id;
            return this;
        }

        public TrainingSessionBuilder WithDefaultExercises(int exercisesCount)
        {
            _exercisesCount = exercisesCount;
            return this; 
        }

        public TrainingSession BuildDomain()
        {
            return new TrainingSession
            {
                Id = _id,
                Name = _name,
                Note = _note,
                CreatedAt = _createdAt,
                DifficultyRating = _difficultyRating,
                DurationMinutes = _durationMinutes,
                UserId = _userId,
                User = _user,
                PerformedExercises = _performedExercises
            };
        }
    }
}
