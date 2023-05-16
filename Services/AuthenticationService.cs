using IdentityService.Interfaces;
using IdentityService.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHashingService passwordHashingService;
        private readonly JwtTokenSettings jwtTokenSettings;

        public AuthenticationService(IUserRepository userRepository, IPasswordHashingService passwordHashingService, JwtTokenSettings jwtTokenSettings)
        {
            this.userRepository = userRepository;
            this.passwordHashingService = passwordHashingService;
            this.jwtTokenSettings = jwtTokenSettings;
        }

        public async Task<bool> AuthenticateUser(string username, string password)
        {
            User user = await userRepository.GetUserByUsername(username);
            if(user != null && passwordHashingService.VerifyPassword(password, user.HashedPassword.Value))
            {
                return true;
            }
            return false;
        }

        public async Task<string> GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username.Value),
                new Claim(ClaimTypes.Email, user.Email.Value),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber.Value),
                new Claim(ClaimTypes.Role, user.Role.Value)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
