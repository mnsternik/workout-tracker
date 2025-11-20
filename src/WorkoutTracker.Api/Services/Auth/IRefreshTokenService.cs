using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Services.Auth
{
    public interface IRefreshTokenService
    {
        public string GenerateRefreshToken();
        public Task AddTokenAsync(UserRefreshToken refreshToken);
        public Task UpdateTokenAsync(UserRefreshToken refreshToken);
        public Task<UserRefreshToken> GetAndValidateTokenAsync(string token);
        public Task RevokeTokenAsync(string token);
    }
}
