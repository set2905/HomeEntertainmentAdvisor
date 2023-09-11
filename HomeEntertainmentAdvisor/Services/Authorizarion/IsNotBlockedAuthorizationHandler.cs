using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HomeEntertainmentAdvisor.Services.Authorizarion
{
    public class IsNotBlockedAuthorizationHandler : AuthorizationHandler<IsNotBlockedRequirement>
    {
        private readonly UserManager<User> userManager;

        public IsNotBlockedAuthorizationHandler(UserManager<User> userManager)
        {
            this.userManager=userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsNotBlockedRequirement requirement)
        {
            User? user = await userManager.GetUserAsync(context.User);
            if (user == null)
            {
                return;
            }

            if (user.IsBlocked==false)
            {
                context.Succeed(requirement);
            }
        }
    }
    public class IsNotBlockedRequirement : IAuthorizationRequirement
    {
    }
}
