using IdentityService.Interfaces;
using IdentityService.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Создание нового пользователя")]
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

        [HttpPut("{userId}")]
        [SwaggerOperation(Summary = "Обновить данные пользователя")]
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
        [SwaggerOperation(Summary = "Удалить пользователя по Id")]
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

        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "Получить пользователя по Id")]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            User user = await userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("{search}")]
        [SwaggerOperation(Summary = "Получить пользователей по запросу")]
        public async Task<IActionResult> SearchUsers(string searchTerm)
        {
            IEnumerable<User> users = await userService.SearchUsers(searchTerm);

            if(User == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Получить всех пользователей")]
        public async Task<IActionResult> GetTotalUsers()
        {
            IEnumerable<User> users = await userService.GetTotalUsers();

            if (User == null)
            {
                return NotFound();
            }

            return Ok(users);
        }
    }
}
