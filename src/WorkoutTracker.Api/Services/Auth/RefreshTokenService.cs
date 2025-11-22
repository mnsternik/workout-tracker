using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Services.Auth
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<UserRefreshToken> GetUserRefreshTokenAsync(string token)
        {
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (storedToken == null)
            {
                throw new UnauthorizedActionException("Invalid refresh token");
            }

            return storedToken;
        }

        public async Task AddTokenAsync(UserRefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken); 
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTokenAsync(UserRefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTokenAsync(UserRefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
        }

        public void ValidateToken(UserRefreshToken refreshToken)
        {
            // Token is expired, should be removed by background job
            if (refreshToken.ExpirationDate < DateTime.UtcNow)
            {
                throw new UnauthorizedActionException("Refresh token expired");
            }

            // Token has been revoked
            if (refreshToken.RevokedDate != null)
            {
                throw new UnauthorizedActionException("Refresh token revoked");
            }

            // Token is valid, but user doesn't exist
            if (refreshToken.User == null)
            {
                throw new UnauthorizedActionException("User associated with token not found");
            }
        }

        public async Task RevokeTokenAsync(string token)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (storedToken != null)
            {
                storedToken.RevokedDate = DateTime.UtcNow;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();
            }
        }
    }
}
