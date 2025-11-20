using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Services.Auth
{
    public interface IJwtTokenService
    {
        public string GenerateJwtToken(ApplicationUser user);
    }
}
