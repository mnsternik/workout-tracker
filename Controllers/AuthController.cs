using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.DTOs.Auth;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Services.Auth;

namespace WorkoutTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;

        const int refreshTokenDurationDays = 7; // TODO: Move it to some config

        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new { Status = "Error", Message = "Email already exists" });
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                UserName = model.Email,
                DisplayName = model.DisplayName,
                SecurityStamp = Guid.NewGuid().ToString(), // Important for invalidating tokens when the password or other security-related data changes
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = $"User creation failed. Errors: {string.Join(", ", errors)}" });
            }

            return StatusCode(StatusCodes.Status201Created, new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Create token
                var tokenInfo = _tokenService.GenerateToken(user);

                // Create refresh token
                var refreshTokenValue = _tokenService.GenerateRefreshToken();
                var refreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenDurationDays);
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

                return Ok(new LoginResponseDto
                {
                    Token = tokenInfo.Token,
                    RefreshToken = refreshTokenValue,
                    Expiration = tokenInfo.Expiration,
                });
            }

            return Unauthorized(new { Status = "Error", Message = "Invalid email or password" });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto model)
        {
            // Find and validate token
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == model.RefreshToken);

            if (storedToken == null)
            {
                return Unauthorized(new { Message = "Invalid refresh token" });
            }

            if (storedToken.ExpirationDate < DateTime.UtcNow)
            {
                _context.RefreshTokens.Remove(storedToken);
                await _context.SaveChangesAsync();
                return Unauthorized(new { Message = "Refresh token expired" });
            }

            if (storedToken.RevokedDate != null)
            {
                return Unauthorized(new { Message = "Refresh token revoked" });
            }

            // Token is valid
            var user = storedToken.User;

            // Token is valid, but user dosen't exist, wierd situation 
            if (user == null)
            {
                return BadRequest(new { Message = "User associated with token not found" });
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

            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            return Ok(new LoginResponseDto
            {
                Token = newTokenInfo.Token,
                Expiration = newTokenInfo.Expiration,
                RefreshToken = newRefreshTokenValue,
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto model)
        {
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == model.RefreshToken);
            if (storedToken != null)
            {
                storedToken.RevokedDate = DateTime.UtcNow;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();
            }

            return Ok(new { Message = "Logout successful" });
        }
    }
}
