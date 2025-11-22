using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Services.Auth
{
    public interface IRefreshTokenService
    {
        public string GenerateRefreshToken();
        public void ValidateToken(UserRefreshToken refreshToken);
        public Task<UserRefreshToken> GetUserRefreshTokenAsync(string token);
        public Task AddTokenAsync(UserRefreshToken refreshToken);
        public Task UpdateTokenAsync(UserRefreshToken refreshToken);
        public Task RevokeTokenAsync(string token);
    }
}
