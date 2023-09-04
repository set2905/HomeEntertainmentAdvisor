using HomeEntertainmentAdvisor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HomeEntertainmentAdvisor.Services
{
    public abstract class AuthServiceBase
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly UserManager<User> userManager;
        private readonly IAuthorizationService authorizationService;

        protected AuthServiceBase(AuthenticationStateProvider authenticationStateProvider, UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            this.authenticationStateProvider=authenticationStateProvider;
            this.userManager=userManager;
            this.authorizationService=authorizationService;
        }

        protected async Task<User?> GetUser(AuthenticationState authState)
        {
            return await userManager.GetUserAsync(authState.User);
        }
        protected async Task<AuthenticationState> GetAuthState()
        {
            return await authenticationStateProvider.GetAuthenticationStateAsync();

        }
        protected async Task<bool> IsUserAuthor(AuthenticationState authState, Review review)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(authState.User, review, "UserIsAuthor");
            return authorizationResult.Succeeded;
        }
        protected async Task<bool> IsUserAdmin(User user)
        {
            return await userManager.IsInRoleAsync(user, "admin");
        }
    }
}
