using IdentityService.Models;

namespace IdentityService.Services
{
    public class UserService
    {
        private readonly List<User> users = new List<User>
        {
            new User { Id = 1, Email = "test@gmail.com", Password="password"}
        };

        public User? Authenticate(string email, string password)
        {
            var user = users.SingleOrDefault(u => u.Email == email && u.Password == password);

            if (user == null) return null;

            return user;
        }

        public User? GetById(int id)
        {
            var user = users.SingleOrDefault(u => u.Id == id);

            if (user == null) return null;

            return user;
        }
    }
}
