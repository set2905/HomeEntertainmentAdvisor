using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Pages;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Services
{
    public class AccountService : AuthServiceBase, IAccountService
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountService(RoleManager<IdentityRole> roleManager,
                              AuthenticationStateProvider authenticationStateProvider,
                              UserManager<User> userManager,
                              IAuthorizationService authorizationService) : base(authenticationStateProvider, userManager, authorizationService)
        {
            this.roleManager=roleManager;

        }

        public async Task<bool> SetBlock(IEnumerable<string> userIds, bool blockValue)
        {
            if (!await IsUserNotBlocked())
                return false;

            List<string> succesfulyChangedIds = new();
            foreach (string userId in userIds)
            {
                User? user = await userManager.FindByIdAsync(userId);
                if (user == null||user.IsBlocked==blockValue) continue;
                user.IsBlocked = blockValue;
                if (!(await userManager.UpdateAsync(user)).Succeeded) continue;
                succesfulyChangedIds.Add(userId);
            }
            return true;
        }

        public async Task<List<User>> GetUsers(int skip = 0, int take = 10)
        {
            return await userManager.Users.Skip(skip).Take(10).ToListAsync();
        }
        public async Task<IdentityResult> OverwriteRoles(string userId, IEnumerable<string> roles)
        {
            if (!await IsUserNotBlocked())
                return IdentityResult.Failed(new IdentityError() { Description="You are blocked" });
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
