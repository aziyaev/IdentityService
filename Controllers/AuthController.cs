using IdentityService.Models;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly UserService userService;
        private readonly TokenService tokenService;

        public AuthController(IConfiguration config, UserService userService, TokenService tokenService)
        {
            this.config = config;
            this.userService = userService;
            this.tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var authenticatedUser = userService.Authenticate(user.Email, user.Password);

            if (authenticatedUser == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            var token = tokenService.GenerateToken(authenticatedUser);
            return Ok(new TokenResponse { Token = token });
        }
    }
}
