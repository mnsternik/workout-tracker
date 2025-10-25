using AutoMapper;
using WorkoutTracker.Api.DTOs.Auth;
using WorkoutTracker.Api.DTOs.ExerciseDefinition;
using WorkoutTracker.Api.DTOs.PerformedExercise;
using WorkoutTracker.Api.DTOs.PerformedSet;
using WorkoutTracker.Api.DTOs.TrainingSession;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // Authentication 
            CreateMap<RegisterDto, ApplicationUser>()
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) // Email is used as UserName for Identity
                    .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName));

            // Exercise Definitions 
            CreateMap<ExerciseDefinitionCreateDto, ExerciseDefinition>()
                .ForMember(dest => dest.MuscleGroupsLinks, opt => opt.MapFrom(src =>
                    src.MuscleGroups.Select(mg => new ExerciseMuscleGroupLink { MuscleGroup = mg })));

            CreateMap<ExerciseDefinitionUpdateDto, ExerciseDefinition>()
                .ForMember(dest => dest.MuscleGroupsLinks, opt => opt.MapFrom(src =>
                    src.MuscleGroups.Select(mg => new ExerciseMuscleGroupLink { MuscleGroup = mg })));

            CreateMap<ExerciseDefinition, ExerciseDefinitionReadDto>()
                .ForMember(dest => dest.MuscleGroups, opt => opt.MapFrom(src => 
                    src.MuscleGroupsLinks.Select(link => link.MuscleGroup)));

            // Performed Sets
            CreateMap<PerformedSetCreateDto, PerformedSet>();
            CreateMap<PerformedSetUpdateDto, PerformedSet>();
            CreateMap<PerformedSet, PerformedSetReadDto>(); 

            // Performed Exercises
            CreateMap<PerformedExerciseCreateDto, PerformedExercise>()
                .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));

            CreateMap<PerformedExerciseUpdateDto, PerformedExercise>()
                .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));

            CreateMap<PerformedExercise, PerformedExerciseReadDto>()
                .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets)); 

            // Training sessions
            CreateMap<TrainingSessionCreateDto, TrainingSession>()
                .ForMember(dest => dest.PerformedExercises, opt => opt.MapFrom(src => src.Exercises));

            CreateMap<TrainingSession, TrainingSessionReadDto>()
                .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.PerformedExercises));

            CreateMap<TrainingSessionUpdateDto, TrainingSession>()
                .ForMember(dest => dest.PerformedExercises, opt => opt.MapFrom(src => src.Exercises));
        }
    }
}
