﻿using System.ComponentModel.DataAnnotations;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.DTOs.Exercise
{
    public record ExerciseUpdateDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; init; } = string.Empty;

        [Required]
        public string Description { get; init; } = string.Empty;

        [Url]
        public string? ImageUrl { get; init; }

        [Required]
        public ExerciseType ExerciseType { get; init; }

        [MinLength(1)]
        public IList<ExerciseMuscleGroupLinkDto> MuscleGroups { get; init; } = new List<ExerciseMuscleGroupLinkDto>();

        [Required]
        public Equipment Equipment { get; init; }

        [Required]
        public DifficultyLevel DifficultyLevel { get; init; }
    }
}
