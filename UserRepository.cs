using IdentityService.Interfaces;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext myDbContext;

        public UserRepository(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        public async Task CreateUser(User user)
        {
            await myDbContext.Users.AddAsync(user);
            await myDbContext.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            myDbContext.Users.Update(user);
            await myDbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(User user)
        {
            myDbContext.Users.Remove(user);
            await myDbContext.SaveChangesAsync();
        }

        public async Task<int> GetTotalUserCount()
        {
            return await myDbContext.Users.CountAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await myDbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await myDbContext.Users.FindAsync(userId);
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return await myDbContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber.Equals(phoneNumber));
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await myDbContext.Users.FirstOrDefaultAsync(u => u.Username.Equals(username));
        }

        public async Task<bool> IsEmailAvailable(string email)
        {
            return await myDbContext.Users.AllAsync(u => !u.Email.Equals(email));
        }

        public async Task<bool> IsPhoneNumberAvailable(string phoneNumber)
        {
            return await myDbContext.Users.AllAsync(u => !u.PhoneNumber.Equals(phoneNumber));
        }

        public async Task<bool> IsUsernameAvailable(string username)
        {
            return await myDbContext.Users.AllAsync(u => !u.Username.Equals(username));
        }

        public async Task<IEnumerable<User>> SearchUsers(string searchTerm)
        {
            return await myDbContext.Users.Where(u => u.Username.Value.Contains(searchTerm) ||
                                                      u.Email.Value.Contains(searchTerm) ||
                                                      u.PhoneNumber.Value.Contains(searchTerm)).ToListAsync();
        }
    }
}
