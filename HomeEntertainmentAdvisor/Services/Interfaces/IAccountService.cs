using HomeEntertainmentAdvisor.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<string?>> GetAllRoles();
        Task<IList<string>> GetUserRoles(User user);
        Task<List<User>> GetUsers(int skip = 0, int take = 10);
        Task SetBlock(IEnumerable<string> userIds, bool blockValue);
        Task<IdentityResult> OverwriteRoles(string userId, IEnumerable<string> roles);

    }
}