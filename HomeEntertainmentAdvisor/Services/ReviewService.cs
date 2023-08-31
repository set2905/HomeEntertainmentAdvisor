using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;

namespace HomeEntertainmentAdvisor.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewsRepo reviewsRepo;
        private readonly IAuthorizationService authorizationService;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly UserManager<User> userManager;

        public ReviewService(IReviewsRepo reviewsRepo, AuthenticationStateProvider authenticationStateProvider, UserManager<User> userManager, IAuthorizationService authorizationService)
        {
            this.reviewsRepo=reviewsRepo;
            this.authenticationStateProvider=authenticationStateProvider;
            this.userManager=userManager;
            this.authorizationService=authorizationService;
        }
        public async Task<Review?> GetById(Guid id)
        {
            return await reviewsRepo.GetById(id);
        }
        public async Task<List<Review>> GetNewest()
        {
            return await reviewsRepo.GetPage(1, 10);
        }
        public async Task<List<Review>> FindReviews(string query, int page = 0, int perPage = 10)
        {
            return await reviewsRepo.GetPage(page, perPage, query);
        }
        public async Task<Guid> SaveReview(Review review)
        {
            AuthenticationState authState = await GetAuthState();
            User? user = await GetUser(authState);

            if (review.Id==default&&(review.Rating.AuthorId==default||review.Rating.Author==null))
            {
                review.Rating.Author=user;
                review.Rating.AuthorId=user.Id;
            }
            else
            {
                var authorizationResult = await authorizationService.AuthorizeAsync(authState.User, review, "UserIsAuthor");
                if (!authorizationResult.Succeeded) throw new UnauthorizedAccessException("You are not the owner of this resource");
            }
            return await reviewsRepo.Save(review);
        }
        private async Task<User?> GetUser(AuthenticationState authState)
        {
            return await userManager.GetUserAsync(authState.User);
        }
        private async Task<AuthenticationState> GetAuthState()
        {
            return await authenticationStateProvider.GetAuthenticationStateAsync();

        }
    }
}
