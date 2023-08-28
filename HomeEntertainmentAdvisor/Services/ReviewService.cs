using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace HomeEntertainmentAdvisor.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewsRepo reviewsRepo;

        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly UserManager<User> userManager;

        public ReviewService(IReviewsRepo reviewsRepo, AuthenticationStateProvider authenticationStateProvider, UserManager<User> userManager)
        {
            this.reviewsRepo=reviewsRepo;
            this.authenticationStateProvider=authenticationStateProvider;
            this.userManager=userManager;
        }

        public async Task<List<Review>> GetNewest()
        {
            return await reviewsRepo.GetPage(1, 10);
        }
        public async Task<List<Review>> FindReviews(string query, int page = 0, int perPage = 10)
        {
            return await reviewsRepo.GetPage(page, perPage, query);
        }
        public async Task EditReview(Review review)
        {
            if (review.Rating.AuthorId==default||review.Rating.Author==null)
            {
                User? author = await GetUser();
                review.Rating.Author=author;
            }
            await reviewsRepo.Save(review);
        }
        private async Task<User?> GetUser()
        {
            AuthenticationState authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            return await userManager.GetUserAsync(authState.User);
        }
    }
}
