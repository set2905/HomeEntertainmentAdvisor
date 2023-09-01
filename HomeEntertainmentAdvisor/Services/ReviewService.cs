using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;


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
        public async Task<(Guid id, bool succeeded, string message)> TrySaveReview(Review review)
        {
            AuthenticationState authState = await GetAuthState();
            User? user = await GetUser(authState);
            if (user==null)
            {
                return (Guid.Empty, false, "User could not be found!");
            }
            if (!TrySetReviewAuthor(user, review))
            {
                if (!await IsUserAuthor(authState, review))
                {
                    return (Guid.Empty, false, "You are not the owner of this resource");
                }
            }
            return (await reviewsRepo.Save(review), true, "Review saved");
        }
        private async Task<bool> IsUserAuthor(AuthenticationState authState, Review review)
        {
            var authorizationResult = await authorizationService.AuthorizeAsync(authState.User, review, "UserIsAuthor");
            return authorizationResult.Succeeded;
        }
        private bool TrySetReviewAuthor(User user, Review review)
        {
            if (review.Id == default && (review.Rating.AuthorId == default || review.Rating.Author == null))
            {
                review.Rating.Author = user;
                review.Rating.AuthorId = user.Id;
                return true;
            }
            else return false;
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
