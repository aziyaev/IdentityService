using IdentityService.Models;

namespace IdentityService.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> AuthenticateUser(string username, string password);
        Task<string> GenerateToken(User user);
    }
}
