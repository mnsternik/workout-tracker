﻿using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Api.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
