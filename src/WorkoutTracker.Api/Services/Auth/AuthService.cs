using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WorkoutTracker.Api.DTOs.Auth;
using WorkoutTracker.Api.Exceptions;
using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IJwtTokenService tokenService,
            IRefreshTokenService refreshTokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _jwtSettings = jwtSettings.Value;
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
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new UnauthorizedActionException("Invalid user credentials");
            }

            // Create JWT token
            var jwtToken = _tokenService.GenerateJwtToken(user);
            var jwtTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);

            // Create refresh token
            var refreshToken = new UserRefreshToken
            {
                UserId = user.Id,
                Token = _refreshTokenService.GenerateRefreshToken(),
                ExpirationDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationDays),
                CreationDate = DateTime.UtcNow
            };

            // Save refresh token
            await _refreshTokenService.AddTokenAsync(refreshToken);

            return new LoginResponseDto
            {
                Token = jwtToken,
                Expiration = jwtTokenExpiration,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto tokenDto)
        {
            var storedRefreshToken = await _refreshTokenService.GetUserRefreshTokenAsync(tokenDto.RefreshToken);
            _refreshTokenService.ValidateToken(storedRefreshToken);

            // Generate new JWT token
            var jwtToken = _tokenService.GenerateJwtToken(storedRefreshToken.User!);
            var jwtTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);

            // Update refresh token
            storedRefreshToken.Token = _refreshTokenService.GenerateRefreshToken();
            storedRefreshToken.ExpirationDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationDays);
            storedRefreshToken.CreationDate = DateTime.UtcNow;

            // Save updated refresh token
            await _refreshTokenService.UpdateTokenAsync(storedRefreshToken);

            return new LoginResponseDto
            {
                Token = jwtToken,
                Expiration = jwtTokenExpiration,
                RefreshToken = storedRefreshToken.Token,
            };
        }

        public async Task LogoutAsync(RefreshTokenRequestDto tokenDto)
        {
            await _refreshTokenService.RevokeTokenAsync(tokenDto.RefreshToken);
        }
    }
}
