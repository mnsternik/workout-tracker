using WorkoutTracker.Api.DTOs.PerformedExercise;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Tests.Builders
{
    public class PerformedExerciseBuilder
    {
        // Used for generating objects with uniqe ID, increased by 1 after every build
        private static int _globalId = 1;

        private int _id = _globalId;
        private int _orderInSession = 1;
        private int _trainingSessionId = 1;
        private int _exerciseDefinitionId = 1;
        private TrainingSession _trainingSession  = null!;
        private ExerciseDefinition _exerciseDefinition  = null!;

        // Declares how many generated PerformedSets with default values each PerformedExercise will have 
        private int _setsCount = 3; 

        public PerformedExerciseBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public PerformedExerciseBuilder WithOrderInSession(int order)
        {
            _orderInSession = order;
            return this;
        }

        public PerformedExerciseBuilder WithTrainingSessionId(int trainingId)
        {
            _trainingSessionId = trainingId;
            return this;
        }

        public PerformedExerciseBuilder WithExerciseDefinitionId(int exerciseDefinitionId)
        {
            _exerciseDefinitionId = exerciseDefinitionId;
            return this; 
        }

        public PerformedExerciseBuilder WithTrainingSession(TrainingSession trainingSession)
        {
            _trainingSession = trainingSession;
            _trainingSessionId = trainingSession.Id;
            return this;
        }

        public PerformedExerciseBuilder WithExerciseDefinition(ExerciseDefinition exerciseDefinition)
        {
            _exerciseDefinition = exerciseDefinition;
            _exerciseDefinitionId = exerciseDefinition.Id;
            return this;
        }

        public PerformedExerciseBuilder WithDefaultSets(int count)
        {
            _setsCount = count;
            return this;
        }
        
        public PerformedExercise BuildDomain() 
        {
            var exercise = new PerformedExercise
            {
                Id = _id,
                OrderInSession = _orderInSession,
                TrainingSessionId = _trainingSessionId,
                TrainingSession = _trainingSession,
                ExerciseDefinitionId = _exerciseDefinitionId,
                ExerciseDefinition = _exerciseDefinition,
            };

            // Creating and assigning new ExerciseDefinition and its ID, if no ExerciseDefinition was passed
            if (_exerciseDefinition == null)
            {
                exercise.ExerciseDefinition = new ExerciseDefinitionBuilder().BuildDomain();
                exercise.ExerciseDefinitionId = exercise.ExerciseDefinition.Id;
            }

            // Creating and assiging list of PerformedSets
            exercise.Sets = new PerformedSetBuilder().BuildManyDomains(_setsCount, exercise);

            _globalId++;
            return exercise;
        }

        public PerformedExerciseCreateDto BuildCreateDto()
        {
            return new PerformedExerciseCreateDto
            {
                OrderInSession = _orderInSession,
                ExerciseDefinitionId = _exerciseDefinitionId,
                Sets = new PerformedSetBuilder().BuildManyCreateDtos(_setsCount)
            };
        }

        public PerformedExerciseUpdateDto BuildUpdateDto()
        {
            var exercise = new PerformedExerciseUpdateDto
            {
                Id = _id,
                OrderInSession = _orderInSession,
                ExerciseDefinitionId = _exerciseDefinitionId,
                Sets = new PerformedSetBuilder().BuildManyUpdateDtos(_setsCount)
            };

            _globalId++;
            return exercise;
        }

        public PerformedExerciseReadDto BuildReadDto()
        {
            var exercise = new PerformedExerciseReadDto
            {
                Id = _id,
                OrderInSession = _orderInSession,
                ExerciseDefinitionId = _exerciseDefinitionId,
                Sets = new PerformedSetBuilder().BuildManyReadDtos(_setsCount)
            };

            _globalId++;
            return exercise;
        }

        public List<PerformedExercise> BuildManyDomains(int count, TrainingSession trainingSession) 
        {
            return Enumerable.Range(1, count)
                .Select(i => new PerformedExerciseBuilder().WithOrderInSession(i).WithTrainingSession(trainingSession).BuildDomain())
                .ToList();
        }

        public List<PerformedExerciseCreateDto> BuildManyCreateDtos(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new PerformedExerciseBuilder().WithOrderInSession(i).BuildCreateDto())
                .ToList();
        }

        public List<PerformedExerciseUpdateDto> BuildManyUpdateDtos(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new PerformedExerciseBuilder().WithOrderInSession(i).BuildUpdateDto())
                .ToList();
        }

        public List<PerformedExerciseReadDto> BuildManyReadDtos(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new PerformedExerciseBuilder().WithOrderInSession(i).BuildReadDto())
                .ToList();
        }
    }
}
