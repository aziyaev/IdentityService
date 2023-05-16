using IdentityService.BaseModels;

namespace IdentityService.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Username Username { get; set; }
        public Role Role { get; set; }
        public Email Email { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        public HashedPassword HashedPassword { get; set; }
        public DateTime? CreatedTime { get; set; }

        public User(Guid id, string username, string hashedPassword, string role, string email, string phoneNumber, DateTime createdTime)
        {
            Id = id;
            Username = new Username(username);
            Role = new Role(role);
            Email = new Email(email);
            PhoneNumber = new PhoneNumber(phoneNumber);
            HashedPassword = new HashedPassword(hashedPassword);
            CreatedTime = createdTime;
        }
    }

    

    public class TokenResponse
    {
        public string? Token { get; set; }
    }
}
