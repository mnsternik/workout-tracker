using WorkoutTracker.Api.DTOs.Exercise;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Tests.Builders
{
    public class ExerciseBuilder
    {
        private int _id = 1;
        private string _name = "Push up";
        private string _description = "Push-up exercise is a close chain kinetic exercise which improves the joint proprioception, joint stability and muscle co-activation around the shoulder joint.";
        private string? _imageUrl = null;
        private ExerciseType _exerciseType = ExerciseType.Strength;
        private ICollection<ExerciseMuscleGroupLink> _muscleGroupsLinks = new List<ExerciseMuscleGroupLink>();
        private Equipment _equipment = Equipment.None;
        private DifficultyLevel _difficultyLevel = DifficultyLevel.Intermediate;

        public ExerciseBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ExerciseBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ExerciseBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public ExerciseBuilder WithImageUrl(string imageUrl)
        {
            _imageUrl = imageUrl;
            return this;
        }
        public ExerciseBuilder WithExerciseType(ExerciseType type)
        {
            _exerciseType = type;
            return this;
        }

        public ExerciseBuilder WithMuscleGroups(params MuscleGroup[] groups)
        {
            _muscleGroupsLinks = groups
                .Select(mg => new ExerciseMuscleGroupLink { MuscleGroup = mg })
                .ToList();
            return this;
        }

        public ExerciseBuilder WithEquipment(Equipment equipment)
        {
            _equipment = equipment;
            return this;
        }

        public ExerciseBuilder WithDifficultyLevel(DifficultyLevel level)
        {
            _difficultyLevel = level;
            return this;
        }

        public Exercise BuildDomain()
        {
            var exercise = new Exercise
            {
                Id = _id,
                Name = _name,
                Description = _description,
                ImageUrl = _imageUrl,
                ExerciseType = _exerciseType,
                Equipment = _equipment,
                DifficultyLevel = _difficultyLevel,
            };

            foreach (var link in _muscleGroupsLinks)
            {
                link.Exercise = exercise;
                link.ExerciseId = exercise.Id;
                exercise.MuscleGroupsLinks.Add(link);
            }

            return exercise;
        }


        public ExerciseReadDto BuildReadDto()
        {
            return new ExerciseReadDto
            {
                Id = _id,
                Name = _name,
                Description = _description,
                ImageUrl = _imageUrl,
                ExerciseType = _exerciseType,
                Equipment = _equipment,
                DifficultyLevel = _difficultyLevel,
                MuscleGroups = _muscleGroupsLinks
                    .Select(mgl => new ExerciseMuscleGroupLinkDto { ExerciseId = _id, MuscleGroup = mgl.MuscleGroup })
                    .ToList()
            };
        }

        public ExerciseCreateDto BuildCreateDto()
        {
            return new ExerciseCreateDto
            {
                Name = _name,
                Description = _description,
                ImageUrl = _imageUrl,
                ExerciseType = _exerciseType,
                Equipment = _equipment,
                DifficultyLevel = _difficultyLevel,
                MuscleGroups = _muscleGroupsLinks
                  .Select(mgl => new ExerciseMuscleGroupLinkDto { ExerciseId = _id, MuscleGroup = mgl.MuscleGroup })
                  .ToList()
            };
        }

        public List<Exercise> BuildManyDomains(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new ExerciseBuilder().WithId(i).WithName("Exercise " + i).BuildDomain())
                .ToList();
        }

        public List<ExerciseReadDto> BuildManyReadDtos(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new ExerciseBuilder().WithId(i).WithName("Exercise " + i).BuildReadDto())
                .ToList();
        }

        public List<ExerciseCreateDto> BuildManyCreateDtos(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new ExerciseBuilder().WithId(i).WithName("Exercise " + i).BuildCreateDto())
                .ToList();
        }
    }
}
