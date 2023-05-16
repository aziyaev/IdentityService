using System.Security.Cryptography;

namespace IdentityService
{
    public class JwtTokenSettings
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? SecretKey { get; set; }
    }
}
