using WorkoutTracker.Api.DTOs.Auth;

namespace WorkoutTracker.Api.Services.Auth
{
    public interface IAuthService
    {
        public Task RegisterUserAsync(RegisterDto registerDto);
        public Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        public Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto tokenDto);
        public Task LogoutAsync(RefreshTokenRequestDto tokenDto);
    }
}
