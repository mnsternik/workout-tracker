using WorkoutTracker.Api.DTOs.Auth;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Services
{
    public interface ITokenService
    {
        LoginResponseDto GenerateToken(ApplicationUser user);
    }
}
