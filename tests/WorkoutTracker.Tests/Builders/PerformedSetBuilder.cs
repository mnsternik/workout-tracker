using WorkoutTracker.Api.DTOs.PerformedSet;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Tests.Builders
{
    public class PerformedSetBuilder
    {
        // Used for generating objects with uniqe ID, increased by 1 after every build
        private static int _globalId = 1; 

        private int _id = _globalId;
        private int _orderInExercise = 1;
        private int? _reps = 12;
        private decimal? _weightKg = 10;
        private int? _durationSeconds = null;
        private int? _distanceMeters = null;
        private int _performedExerciseId = 1;
        private PerformedExercise _performedExercise  = null!;

        public PerformedSetBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public PerformedSetBuilder WithOrderInExercise(int order)
        {
            _orderInExercise = order;
            return this;
        }

        public PerformedSetBuilder WithReps(int reps)
        {
            _reps = reps;
            return this;
        }

        public PerformedSetBuilder WithWeightKg(int weight)
        {
            _weightKg = weight;
            return this;
        }

        public PerformedSetBuilder WithDurationSeconds(int duration)
        {
            _durationSeconds = duration;
            return this;
        }

        public PerformedSetBuilder WithDistanceMeters(int distance)
        {
            _distanceMeters = distance;
            return this;
        }

        public PerformedSetBuilder WithPerformedExerciseId(int exerciseId)
        {
            _performedExerciseId = exerciseId;
            return this;
        }

        public PerformedSetBuilder WithPerformedExercise(PerformedExercise exercise)
        {
            _performedExercise = exercise; 
            _performedExerciseId = exercise.Id;
            return this; 
        }

        public PerformedSet BuildDomain()
        {
            var performedSet = new PerformedSet
            {
                Id = _id,
                OrderInExercise = _orderInExercise,
                Reps = _reps,
                WeightKg = _weightKg,
                DurationSeconds = _durationSeconds,
                DistanceMeters = _distanceMeters,
                PerformedExerciseId = _performedExerciseId,
                PerformedExercise = _performedExercise
            }; 

            _globalId++; 
            return performedSet;
        }

        public PerformedSetCreateDto BuildCreateDto()
        {
            return new PerformedSetCreateDto
            {
                OrderInExercise = _orderInExercise,
                Reps = _reps,
                WeightKg = _weightKg,
                DurationSeconds = _durationSeconds,
                DistanceMeters = _distanceMeters,
            };
        }

        public PerformedSetReadDto BuildReadDto()
        {
            var performedSet = new PerformedSetReadDto
            {
                Id = _id,
                OrderInExercise = _orderInExercise,
                Reps = _reps,
                WeightKg = _weightKg,
                DurationSeconds = _durationSeconds,
                DistanceMeters = _distanceMeters,
            };

            _globalId++;
            return performedSet;
        }

        public List<PerformedSet> BuildManyDomains(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new PerformedSetBuilder().WithOrderInExercise(i).BuildDomain())
                .ToList();
        }

        public List<PerformedSetCreateDto> BuildManyCreateDtos(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new PerformedSetBuilder().WithOrderInExercise(i).BuildCreateDto())
                .ToList();
        }

        public List<PerformedSetReadDto> BuildManyReadDtos(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new PerformedSetBuilder().WithOrderInExercise(i).BuildReadDto())
                .ToList();
        }
    }
}
