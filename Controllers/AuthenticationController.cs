using IdentityService.Interfaces;
using IdentityService.Models;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IUserService userService; 

        public AuthenticationController(IAuthenticationService authenticationService, IUserService userService)
        {
            this.authenticationService = authenticationService;
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            bool isAuthenticated = await authenticationService.AuthenticateUser(model.Username, model.Password);

            if (!isAuthenticated)
            {
                return Unauthorized();
            }

            User user = await userService.GetUserByUsername(model.Username);

            string token = await authenticationService.GenerateToken(user);

            return Ok(new {Token = token});
        }
    }
}
