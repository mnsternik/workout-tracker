using WorkoutTracker.Api.DTOs.ExerciseDefinition;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Tests.Builders
{
    public class ExerciseDefinitionBuilder
    {
        // Used for generating objects with uniqe ID, increased by 1 after every build
        private static int _globalId = 1; 

        private int _id = _globalId;
        private string _name = $"Exercise definition {_globalId}"; // Name must be uniqe
        private string _description = "Test exercise definition's description";
        private string? _imageUrl = null;
        private ExerciseType _exerciseType = ExerciseType.Strength;
        private ICollection<ExerciseMuscleGroupLink> _muscleGroupsLinks = [];
        private Equipment _equipment = Equipment.None;
        private DifficultyLevel _difficultyLevel = DifficultyLevel.Intermediate;

        public ExerciseDefinitionBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ExerciseDefinitionBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ExerciseDefinitionBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public ExerciseDefinitionBuilder WithImageUrl(string imageUrl)
        {
            _imageUrl = imageUrl;
            return this;
        }
        public ExerciseDefinitionBuilder WithExerciseType(ExerciseType type)
        {
            _exerciseType = type;
            return this;
        }

        public ExerciseDefinitionBuilder WithMuscleGroups(params MuscleGroup[] groups)
        {
            _muscleGroupsLinks = groups
                .Select(mg => new ExerciseMuscleGroupLink { MuscleGroup = mg })
                .ToList();
            return this;
        }

        public ExerciseDefinitionBuilder WithEquipment(Equipment equipment)
        {
            _equipment = equipment;
            return this;
        }

        public ExerciseDefinitionBuilder WithDifficultyLevel(DifficultyLevel level)
        {
            _difficultyLevel = level;
            return this;
        }

        public ExerciseDefinition BuildDomain()
        {
            var exercise = new ExerciseDefinition
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
                link.ExerciseDefinition = exercise;
                link.ExerciseDefinitionId = exercise.Id;
                exercise.MuscleGroupsLinks.Add(link);
            }

            _globalId++; 
            return exercise;
        }
        public ExerciseDefinitionReadDto BuildReadDto()
        {
            var exercise =  new ExerciseDefinitionReadDto
            {
                Id = _id,
                Name = _name,
                Description = _description,
                ImageUrl = _imageUrl,
                ExerciseType = _exerciseType,
                Equipment = _equipment,
                DifficultyLevel = _difficultyLevel,
                MuscleGroups = _muscleGroupsLinks.Select(link => link.MuscleGroup).ToList(),
            };

            _globalId++;
            return exercise;
        }

        public ExerciseDefinitionCreateDto BuildCreateDto()
        {
            return new ExerciseDefinitionCreateDto
            {
                Name = _name,
                Description = _description,
                ImageUrl = _imageUrl,
                ExerciseType = _exerciseType,
                Equipment = _equipment,
                DifficultyLevel = _difficultyLevel,
                MuscleGroups = _muscleGroupsLinks.Select(link => link.MuscleGroup).ToList(),
            };
        }

        public ExerciseDefinitionUpdateDto BuildUpdateDto()
        {
            var exercise = new ExerciseDefinitionUpdateDto
            {
                Id = _id,
                Name = _name,
                Description = _description,
                ImageUrl = _imageUrl,
                ExerciseType = _exerciseType,
                Equipment = _equipment,
                DifficultyLevel = _difficultyLevel,
                MuscleGroups = _muscleGroupsLinks.Select(link => link.MuscleGroup).ToList(),
            };

            _globalId++;
            return exercise;
        }

        public List<ExerciseDefinition> BuildManyDomains(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new ExerciseDefinitionBuilder().WithName("Exercise " + i).BuildDomain())
                .ToList();
        }

        public List<ExerciseDefinitionReadDto> BuildManyReadDtos(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new ExerciseDefinitionBuilder().WithName("Exercise " + i).BuildReadDto())
                .ToList();
        }

        public List<ExerciseDefinitionCreateDto> BuildManyCreateDtos(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new ExerciseDefinitionBuilder().WithName("Exercise " + i).BuildCreateDto())
                .ToList();
        }
    }
}
