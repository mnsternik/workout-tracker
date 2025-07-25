﻿using AutoMapper;
using WorkoutTracker.Api.DTOs.Auth;
using WorkoutTracker.Api.DTOs.Exercise;
using WorkoutTracker.Api.DTOs.Training.Exercise;
using WorkoutTracker.Api.DTOs.Training.Set;
using WorkoutTracker.Api.DTOs.Training.TrainingSession;
using WorkoutTracker.Api.DTOs.TrainingSession.TrainingSession;
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
            CreateMap<ExerciseCreateDto, Exercise>()
                .ForMember(dest => dest.MuscleGroupsLinks, opt => opt.Ignore()); // MuscleGroupsLinks are handled separately after mapping

            CreateMap<ExerciseUpdateDto, Exercise>()
                //.ForMember(dest => dest.MuscleGroupsLinks,
                //    opt => opt.MapFrom(src => src.MuscleGroups)); ;
                .ForMember(dest => dest.MuscleGroupsLinks, opt => opt.Ignore());

            CreateMap<Exercise, ExerciseReadDto>()
                .ForMember(dest => dest.MuscleGroups,
                    opt => opt.MapFrom(src => src.MuscleGroupsLinks));

            // ExerciseMuscleGroupLink
            CreateMap<ExerciseMuscleGroupLink, ExerciseMuscleGroupLinkDto>();
            CreateMap<ExerciseMuscleGroupLinkDto, ExerciseMuscleGroupLink>();

            // Training session's Sets
            CreateMap<TrainingSetCreateDto, TrainingSet>(); 
            CreateMap<TrainingSet, TrainingSetReadDto>(); 

            // Training sessions's Exercises
            CreateMap<TrainingExerciseCreateDto, TrainingExercise>()
                .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));

            CreateMap<TrainingExercise, TrainingExerciseReadDto>()
                .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets)); 

            // Training sessions
            CreateMap<TrainingSessionCreateDto, TrainingSession>()
                .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.Exercises));

            CreateMap<TrainingSession, TrainingSessionReadDto>()
                .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.Exercises));

            CreateMap<TrainingSessionUpdateDto, TrainingSession>()
                .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.Exercises));
        }
    }
}
