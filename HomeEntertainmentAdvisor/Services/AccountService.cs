using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
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
        /// <summary>
        /// Sets block status to provided users
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="blockValue"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets <paramref name="take"/> users starting from <paramref name="skip"/> index
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUsers(int skip = 0, int take = 10)
        {
            return await userManager.Users.OrderBy(x=>x.UserName).Skip(skip).Take(10).ToListAsync();
        }
        /// <summary>
        /// Overwrites user roles with specified roles
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get all existing roles 
        /// </summary>
        /// <returns></returns>
        public async Task<List<string?>> GetAllRoles()
        {
            List<string?> roles = await roleManager.Roles.Select(x => x.Name).ToListAsync();
            return roles;
        }
        /// <summary>
        /// Gets all roles on a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IList<string>> GetUserRoles(User user)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            return roles;
        }
    }
}
