using IdentityService.Interfaces;
using IdentityService.Models;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace IdentityService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHashingService passwordHashingService;
        private readonly AppSettings appSettings;

        public UserService(IUserRepository userRepository, IPasswordHashingService passwordHashingService, IOptions<AppSettings> appSettings)
        {
            this.userRepository = userRepository;
            this.passwordHashingService= passwordHashingService;
            this.appSettings = appSettings.Value;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await userRepository.GetUserById(userId);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await userRepository.GetUserByUsername(username);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await userRepository.GetUserByEmail(email);
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return await userRepository.GetUserByPhoneNumber(phoneNumber);
        }

        public bool IsUsernameValid(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            if (username.Length > appSettings.MaxUsernameLenght)
            {
                return false;
            }

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                return false;
            }

            return true;
        }

        public bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            if (!Regex.IsMatch(email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
            {
                return false;
            }

            return true;
        }

        public bool IsPhoneNumberValid(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false;
            }

            if (!Regex.IsMatch(phoneNumber, @"^\+[0-9]{1,3}-[0-9]{1,14}$"))
            {
                return false;
            }

            return true;
        }

        public bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (!Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"))
            {
                return false;
            }

            return true;
        }


        public async Task CreateUser(string username, string password, string email, string phoneNumber, string role)
        {
            if(!(IsUsernameValid(username) || await userRepository.IsUsernameAvailable(username)))
            {
                throw new InvalidOperationException("Username is already taken.");
            }

            if (!(IsEmailValid(email) || await userRepository.IsEmailAvailable(email)))
            {
                throw new InvalidOperationException("Email is already taken.");
            }

            if (!(IsPhoneNumberValid(phoneNumber) || await userRepository.IsPhoneNumberAvailable(phoneNumber)))
            {
                throw new InvalidOperationException("PhoneNumber is already taken.");
            }

            if (!IsPasswordValid(password))
            {
                throw new InvalidOperationException("Password is not valid");
            }

            User user = new User(Guid.NewGuid(),
                username,
                passwordHashingService.HashPassword(password),
                role,
                email,
                phoneNumber,
                DateTime.Now);

            await userRepository.CreateUser(user);
        }

        public async Task UpdateUser(Guid userId, string username, string password, string email, string phoneNumber, string role)
        {
            User existingUser = await userRepository.GetUserById(userId);
            if(existingUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            if (!(IsUsernameValid(username) || await userRepository.IsUsernameAvailable(username)))
            {
                throw new InvalidOperationException("Username is already taken.");
            }

            if (!(IsEmailValid(email) || await userRepository.IsEmailAvailable(email)))
            {
                throw new InvalidOperationException("Email is already taken.");
            }

            if (!(IsPhoneNumberValid(phoneNumber) || await userRepository.IsPhoneNumberAvailable(phoneNumber)))
            {
                throw new InvalidOperationException("PhoneNumber is already taken.");
            }

            if(! IsPasswordValid(password))
            {
                throw new InvalidOperationException("Password is not valid");
            }

            existingUser.Username = new BaseModels.Username(username);
            existingUser.Email = new BaseModels.Email(email);
            existingUser.PhoneNumber = new BaseModels.PhoneNumber(phoneNumber);
            existingUser.Role = new BaseModels.Role(role);
            existingUser.HashedPassword = new BaseModels.HashedPassword(passwordHashingService.HashPassword(password));

            await userRepository.UpdateUser(existingUser);
        }

        public async Task DeleteUser(Guid userId)
        {
            User existingUser = await userRepository.GetUserById(userId);
            if (existingUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            await userRepository.DeleteUser(existingUser);
        }

        public async Task<IEnumerable<User>> SearchUsers(string searchTerm)
        {
            return await userRepository.SearchUsers(searchTerm);
        }

        public async Task<int> GetTotalUserCount()
        {
            return await userRepository.GetTotalUserCount();
        }
    }
}
