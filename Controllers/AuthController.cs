using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Api.DTOs.Auth;
using WorkoutTracker.Api.Services.Auth;

namespace WorkoutTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService; 

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            await _authService.RegisterUserAsync(registerDto);
            return StatusCode(StatusCodes.Status201Created, new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var loginResponseDto = await _authService.LoginAsync(loginDto);
            return Ok(loginResponseDto);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto tokenDto)
        {
           var loginResponseDto = await _authService.RefreshTokenAsync(tokenDto);
            return Ok(loginResponseDto);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto tokenDto)
        {
            await _authService.LogoutAsync(tokenDto);
            return Ok(new { Message = "Logout successful" });
        }
    }
}
