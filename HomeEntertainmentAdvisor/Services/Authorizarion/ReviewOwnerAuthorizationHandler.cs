using HomeEntertainmentAdvisor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HomeEntertainmentAdvisor.Services.Authorizarion
{
    public class ReviewOwnerAuthorizationHandler : AuthorizationHandler<UserIsAuthorRequirement, Review>
    {
        private readonly UserManager<User> userManager;

        public ReviewOwnerAuthorizationHandler(UserManager<User> userManager)
        {
            this.userManager=userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            UserIsAuthorRequirement requirement,
            Review resource)
        {
            User? user = await userManager.GetUserAsync(context.User);
            if (user!=null&&user.Id == resource.Rating.AuthorId)
            {
                context.Succeed(requirement);
            }
        }
    }
    public class UserIsAuthorRequirement : IAuthorizationRequirement { }
}
