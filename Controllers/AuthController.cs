using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Api.DTOs.Auth;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService; 

        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
                return StatusCode(StatusCodes.Status409Conflict, new { Status = "Error", Message = "Email already exists" });

            ApplicationUser user = new()
            {
                Email = model.Email,
                UserName = model.Email,
                DisplayName = model.DisplayName,
                SecurityStamp = Guid.NewGuid().ToString(), // Ważne dla unieważniania tokenów przy zmianie hasła itp.
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
                var tokenInfo = _tokenService.GenerateToken(user);
                return Ok(tokenInfo); 
            }

            return Unauthorized(new { Status = "Error", Message = "Invalid email or password" });
        }
    }
}
