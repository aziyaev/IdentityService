using IdentityService.Models;

namespace IdentityService.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(Guid userId);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByPhoneNumber(string phoneNumber);

        Task CreateUser(string username, string password, string email, string phoneNumber, string role);
        Task UpdateUser(Guid userId, string username, string password, string email, string phoneNumber, string role);
        Task DeleteUser(Guid userId);

        bool IsUsernameValid(string username);
        bool IsEmailValid(string email);
        bool IsPhoneNumberValid(string email);
        bool IsPasswordValid(string password);

        Task<IEnumerable<User>> SearchUsers(string searchTerm);
        Task<int> GetTotalUserCount();
    }
}
