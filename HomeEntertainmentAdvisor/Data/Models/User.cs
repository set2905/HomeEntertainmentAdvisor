using Microsoft.AspNetCore.Identity;

namespace HomeEntertainmentAdvisor.Data.Models
{
    public class User : IdentityUser
    {
        public bool IsBlocked { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
