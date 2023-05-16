using Microsoft.AspNetCore.Mvc.Razor;

namespace IdentityService
{
    public class AppSettings
    {
        public int MaxUsernameLenght { get; set; }
        public int MaxRoleLenght { get; set; }
        public int MaxPasswordLenght { get; set; }
    }
}
