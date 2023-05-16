using IdentityService.Models;

namespace IdentityService.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(Guid userId);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(User user);
        Task<bool> IsUsernameAvailable(string username);
        Task<bool> IsEmailAvailable(string email);
        Task<bool> IsPhoneNumberAvailable(string phoneNumber);

        Task<IEnumerable<User>> SearchUsers(string searchTerm);
        Task<IEnumerable<User>> GetTotalUsers();
        Task<int> GetTotalUserCount();
    }
}
