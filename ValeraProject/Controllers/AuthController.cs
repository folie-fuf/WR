using Microsoft.AspNetCore.Mvc;
using ValeraProject.DTOs;
using ValeraProject.Services;

namespace ValeraProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);
            if (result == null)
                return BadRequest("Email or username already exists");

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            var result = await _authService.Login(loginDto);
            if (result == null)
                return Unauthorized("Invalid email or password");

            return Ok(result);
        }
    }
}