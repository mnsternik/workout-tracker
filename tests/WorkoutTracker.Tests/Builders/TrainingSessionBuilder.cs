using WorkoutTracker.Api.DTOs.TrainingSession;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Tests.Builders
{
    public class TrainingSessionBuilder
    {
        // Used for generating objects with uniqe ID, increased by 1 after every build
        private static int _globalId = 1; 

        private int _id = _globalId;
        private string _name = "My training session";
        private string? _note = "Notes to my trainig session";
        private DateTime _createdAt = DateTime.UtcNow;
        private DifficultyRating? _difficultyRating = DifficultyRating.Easy;
        private int? _durationMinutes = 45;
        private string _userId = new Guid().ToString();
        private ApplicationUser _user = null!; 
        private ICollection<PerformedExercise> _performedExercises = [];

        // Declares how many generated PerformedExercises with default values each TrainingSession will have 
        private int _exercisesCount = 3; 

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
            var training = new TrainingSession
            {
                Id = _id,
                Name = _name,
                Note = _note,
                CreatedAt = _createdAt,
                DifficultyRating = _difficultyRating,
                DurationMinutes = _durationMinutes,
                UserId = _userId,
                User = _user, 
                PerformedExercises = _performedExercises,
            };
            

            // If there was no provided PerformedExercises list, than create one with default values
            if (!_performedExercises.Any())
            {
                training.PerformedExercises = new PerformedExerciseBuilder().BuildManyDomains(_exercisesCount, training);
            }

            // If there was no provided User (author), then create one with default values  
            if (_user == null)
            {
                training.User = new ApplicationUserBuilder().BuildDomain();
                training.UserId = training.User.Id;
            }

            _globalId++;
            return training;
        }

        public TrainingSessionCreateDto BuildCreateDto()
        {
            return new TrainingSessionCreateDto
            {
                Name = _name,
                Note = _note,
                DifficultyRating = _difficultyRating,
                DurationMinutes = _durationMinutes,
                Exercises = new PerformedExerciseBuilder().BuildManyCreateDtos(_exercisesCount),
            };
        }

        public TrainingSessionUpdateDto BuildUpdateDto()
        {
            return new TrainingSessionUpdateDto
            {
                Id = _id,
                Name = _name,
                Note = _note,
                DifficultyRating = _difficultyRating,
                DurationMinutes = _durationMinutes,
                Exercises = new PerformedExerciseBuilder().BuildManyUpdateDtos(_exercisesCount),
            };
        }

        public TrainingSessionReadDto BuildReadDto()
        {
            return new TrainingSessionReadDto
            {
                Id = _id,
                Name = _name,
                Note = _note,
                DifficultyRating = _difficultyRating,
                DurationMinutes = _durationMinutes,
                Exercises = new PerformedExerciseBuilder().BuildManyReadDtos(_exercisesCount),
            };
        }

        public List<TrainingSession> BuildManyDomains(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new TrainingSessionBuilder().WithName("Training session " + i).BuildDomain())
                .ToList();
        }

        public List<TrainingSessionCreateDto> BuildManyCreateDtos(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new TrainingSessionBuilder().WithName("Training session " + i).BuildCreateDto())
                .ToList();
        }

        public List<TrainingSessionUpdateDto> BuildManyUpdateDtos(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new TrainingSessionBuilder().WithName("Training session " + i).BuildUpdateDto())
                .ToList();
        }

        public List<TrainingSessionReadDto> BuildManyReadDtos(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new TrainingSessionBuilder().WithName("Training session " + i).BuildReadDto())
                .ToList();
        }
    }
}
