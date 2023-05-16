using IdentityService.Interfaces;
using IdentityService.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequestModel model)
        {
            try
            {
                await userService.CreateUser(model.UserName, model.Password, model.Email, model.PhoneNumber, model.Role);

                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            User user = await userService.GetUserById(userId);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, UpdateUserRequestModel model)
        {
            User user = await userService.GetUserById(userId);

            if(user == null)
            {
                return NotFound();
            }

            await userService.UpdateUser(userId, model.UserName, model.Password, model.Email, model.PhoneNumber, model.Role);

            return Ok();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            User user = await userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            await userService.DeleteUser(userId);
            return Ok();
        }
    }
}
