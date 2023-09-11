using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Pages;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager=userManager;
            this.roleManager=roleManager;
        }
        public async Task SetBlock(IEnumerable<string> userIds, bool blockValue)
        {
            List<string> succesfulyChangedIds = new();
            foreach (string userId in userIds)
            {
                User? user = await userManager.FindByIdAsync(userId);
                if (user == null||user.IsBlocked==blockValue) continue;
                user.IsBlocked = blockValue;
                if (!(await userManager.UpdateAsync(user)).Succeeded) continue;
                succesfulyChangedIds.Add(userId);
            }
        }

        public async Task<List<User>> GetUsers(int skip = 0, int take = 10)
        {
            return await userManager.Users.Skip(skip).Take(10).ToListAsync();
        }
        public async Task<IdentityResult> OverwriteRoles(string userId, IEnumerable<string> roles)
        {
            User? user = await userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError() { Description="User not found" });
            IList<string> rolesToRemove = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, rolesToRemove.ToArray());
            var result = await userManager.AddToRolesAsync(user, roles);
            return result;
        }
        public async Task<List<string?>> GetAllRoles()
        {
            List<string?> roles = await roleManager.Roles.Select(x => x.Name).ToListAsync();
            return roles;
        }
        public async Task<IList<string>> GetUserRoles(User user)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            return roles;
        }
    }
}
