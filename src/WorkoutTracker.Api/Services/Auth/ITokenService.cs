using WorkoutTracker.Api.DTOs.Auth;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Services.Auth
{
    public interface ITokenService
    {
        LoginResponseDto GenerateToken(ApplicationUser user);
        string GenerateRefreshToken();
    }
}
