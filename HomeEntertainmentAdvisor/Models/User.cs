using Microsoft.AspNetCore.Identity;

namespace HomeEntertainmentAdvisor.Models
{
    public class User : IdentityUser
    {
        public bool IsBlocked { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastCacheUpdate { get; set; }
        public int CachedLikes { get; set; }
    }
}
