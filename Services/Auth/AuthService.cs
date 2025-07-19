using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.DTOs.Auth;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        const int refreshTokenDurationDays = 7; // TODO: Move it to some config

        public AuthService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task RegisterUserAsync(RegisterDto registerDto)
        {
            var emailExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (emailExists != null)
            {
                throw new EntityAlreadyExistsException("User with this email already exists");
            }

            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                SecurityStamp = Guid.NewGuid().ToString(), // Important for invalidating tokens when the password or other security-related data changes
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new CreateUserAccountException($"User creation failed. Errors: {string.Join(", ", errors)}");
            }
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                // Create token
                var tokenInfo = _tokenService.GenerateToken(user);

                // Create refresh token
                var refreshTokenValue = _tokenService.GenerateRefreshToken();
                var refreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenDurationDays); // TODO: Read it from the appsettings.json 
                var refreshToken = new UserRefreshToken
                {
                    UserId = user.Id,
                    Token = refreshTokenValue,
                    ExpirationDate = refreshTokenExpiry,
                    CreationDate = DateTime.UtcNow
                };

                // Store refresh token
                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();

                return new LoginResponseDto
                {
                    Token = tokenInfo.Token,
                    RefreshToken = refreshTokenValue,
                    Expiration = tokenInfo.Expiration,
                };
            }
            else
            {
                throw new UnauthorizedActionException("Invalid user credentials");
            }
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto tokenDto)
        {
            // Find token
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == tokenDto.RefreshToken);

            // Token validation
            if (storedToken == null)
            {
                throw new UnauthorizedActionException("Invalid refresh token");
            }

            if (storedToken.ExpirationDate < DateTime.UtcNow)
            {
                // If token is expired, remove it
                _context.RefreshTokens.Remove(storedToken);
                await _context.SaveChangesAsync();
                throw new UnauthorizedActionException("Refresh token expired");
            }

            if (storedToken.RevokedDate != null)
            {
                throw new UnauthorizedActionException("Refresh token revoked");
            }

            // Token is valid
            var user = storedToken.User;

            // Token is valid, but user dosen't exist
            if (user == null)
            {
                throw new UnauthorizedActionException("User associated with token not found" );
            }

            // Generete new token
            var newTokenInfo = _tokenService.GenerateToken(user);

            // Rotation of refresh token
            var newRefreshTokenValue = _tokenService.GenerateRefreshToken();
            var newRefreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenDurationDays);

            // Update refresh token
            storedToken.Token = newRefreshTokenValue;
            storedToken.ExpirationDate = newRefreshTokenExpiry;
            storedToken.CreationDate = DateTime.UtcNow;

            // Save new refresh token
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                Token = newTokenInfo.Token,
                Expiration = newTokenInfo.Expiration,
                RefreshToken = newRefreshTokenValue,
            };
        }

        public async Task LogoutAsync(RefreshTokenRequestDto tokenDto)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == tokenDto.RefreshToken);

            if (storedToken != null)
            {
                storedToken.RevokedDate = DateTime.UtcNow;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();
            }
        }
    }
}
