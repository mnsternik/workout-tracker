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

            // Predefined Exercises
            CreateMap<ExerciseDefinitionCreateDto, ExerciseDefinition>()
                .ForMember(dest => dest.MuscleGroupsLinks, opt => opt.Ignore()); // MuscleGroupsLinks are handled separately after mapping

            CreateMap<ExerciseDefinitionUpdateDto, ExerciseDefinition>()
                .ForMember(dest => dest.MuscleGroupsLinks, opt => opt.Ignore());

            CreateMap<ExerciseDefinition, ExerciseDefinitionReadDto>()
                .ForMember(dest => dest.MuscleGroups,
                    opt => opt.MapFrom(src => src.MuscleGroupsLinks));

            // ExerciseMuscleGroupLink
            CreateMap<ExerciseMuscleGroupLink, ExerciseDefinitionMuscleGroupLinkDto>();
            CreateMap<ExerciseDefinitionMuscleGroupLinkDto, ExerciseMuscleGroupLink>();

            // Training session's Sets
            CreateMap<PerformedSetCreateDto, PerformedSet>(); 
            CreateMap<PerformedSet, PerformedSetReadDto>(); 

            // Training sessions's Exercises
            CreateMap<PerformedExerciseCreateDto, PerformedExercise>()
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
