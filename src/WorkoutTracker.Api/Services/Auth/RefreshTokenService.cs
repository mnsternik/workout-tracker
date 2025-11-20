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

        public async Task AddTokenAsync(UserRefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken); 
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTokenAsync(UserRefreshToken refreshToken)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Id == refreshToken.Id);

            if (storedToken == null)
            {
                throw new UnauthorizedActionException("Invalid refresh token");
            }

            storedToken.Token = refreshToken.Token;
            storedToken.ExpirationDate = refreshToken.ExpirationDate;

            await _context.SaveChangesAsync();
        }

        public async Task<UserRefreshToken> GetAndValidateTokenAsync(string token)
        {
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (storedToken == null)
            {
                throw new UnauthorizedActionException("Invalid refresh token");
            }

            // If token is expired, remove it
            if (storedToken.ExpirationDate < DateTime.UtcNow)
            {
                _context.RefreshTokens.Remove(storedToken);
                await _context.SaveChangesAsync();
                throw new UnauthorizedActionException("Refresh token expired");
            }

            // Token has been revoked
            if (storedToken.RevokedDate != null)
            {
                throw new UnauthorizedActionException("Refresh token revoked");
            }

            // Token is valid
            var user = storedToken.User;

            // Token is valid, but user dosen't exist
            if (user == null)
            {
                throw new UnauthorizedActionException("User associated with token not found");
            }

            return storedToken;
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
