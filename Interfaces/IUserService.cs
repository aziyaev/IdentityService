using IdentityService.Models;

namespace IdentityService.Interfaces
{
    public interface IUserService
    {
        User? Authenticate(string email, string password);
        User? GetById(int id);
    }
}
