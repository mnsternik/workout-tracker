using AutoMapper;
using WorkoutTracker.Api.DTOs.Auth;
using WorkoutTracker.Api.DTOs.Exercise;
using WorkoutTracker.Api.DTOs.Training.Exercise;
using WorkoutTracker.Api.DTOs.Training.Set;
using WorkoutTracker.Api.DTOs.Training.TrainingSession;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // --- Authentication Mappings ---
            CreateMap<RegisterDto, ApplicationUser>()
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) // Email is used as UserName for Identity
                    .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName));

            // --- Exercise Mappings (Predefined Exercises) ---
            CreateMap<ExerciseCreateDto, Exercise>()
                .ForMember(dest => dest.MuscleGroupsLinks, opt => opt.Ignore()); // MuscleGroupsLinks are handled separately after mapping

            CreateMap<Exercise, ExerciseReadDto>()
                    .ForMember(dest => dest.MuscleGroups, opt => opt.MapFrom(src => src.MuscleGroupsLinks.Select(link => link.MuscleGroup).ToList()));

            // --- Training Set Mappings ---
            CreateMap<TrainingSetCreateDto, TrainingSet>(); 
            CreateMap<TrainingSet, TrainingSetReadDto>(); 

            // --- Training Exercise Mappings ---
            CreateMap<TrainingExerciseCreateDto, TrainingExercise>()
                .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));

            CreateMap<TrainingExercise, TrainingExerciseReadDto>()
                .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets)); 

            // --- Training Session Mappings ---
            CreateMap<TrainingSessionCreateDto, TrainingSession>()
                .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.Exercises));

            CreateMap<TrainingSession, TrainingSessionReadDto>()
                .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.Exercises)) 
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id)); // Assuming User navigation property is loaded

            // TODO: Create a list of training sessions

        }
    }
}
