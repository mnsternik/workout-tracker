﻿using Microsoft.AspNetCore.Identity;

namespace WorkoutTracker.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
    }
}
