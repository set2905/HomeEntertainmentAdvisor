using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;

        public AccountService(UserManager<User> userManager)
        {
            this.userManager=userManager;
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
        public async Task<List<string>> DeleteUsers(IEnumerable<string> userIds)
        {
            List<string> succesfulyDeletedUsers = new();
            foreach (string userId in userIds)
            {
                User? user = await userManager.FindByIdAsync(userId);
                if (user == null) continue;
                if (!(await userManager.DeleteAsync(user)).Succeeded) continue;
                succesfulyDeletedUsers.Add(userId);
            }
            return succesfulyDeletedUsers;
        }
        public async Task<List<User>> GetUsers(int skip = 0, int take = 10)
        {
            return await userManager.Users.Skip(skip).Take(10).ToListAsync();
        }
    }
}
