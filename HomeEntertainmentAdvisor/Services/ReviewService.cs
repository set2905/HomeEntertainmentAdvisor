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
        private readonly IRatingRepo ratingRepo;
        private readonly IMediaPiecesRepo mediaRepo;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly UserManager<User> userManager;

        public ReviewService(IReviewsRepo reviewsRepo, IRatingRepo ratingRepo, IMediaPiecesRepo mediaRepo, AuthenticationStateProvider authenticationStateProvider, UserManager<User> userManager)
        {
            this.reviewsRepo=reviewsRepo;
            this.ratingRepo=ratingRepo;
            this.authenticationStateProvider=authenticationStateProvider;
            this.userManager=userManager;
            this.mediaRepo=mediaRepo;
        }

        public async Task<List<Review>> GetNewest()
        {
            return await reviewsRepo.GetPage(1, 10);
        }
        public async Task<List<Review>> Find(string query, int page = 1, int perPage = 10)
        {
            return await reviewsRepo.GetPage(page, perPage, query);
        }
        public async Task CreateReview()
        {
            var media = (await mediaRepo.GetAll()).First();

            Guid ratingId = await ratingRepo.Save(new Rating()
            {
                MediaPieceId=media.Id,
            });
            Rating rating = await ratingRepo.GetById(ratingId);
            Review rev = new Review
            {
                Name="test review",
                Content="SearchTest",
                RatingId=ratingId,
                Rating=rating,
            };
            await reviewsRepo.Save(rev);
        }
        private async Task<User?> GetUser()
        {
            AuthenticationState authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            return await userManager.GetUserAsync(authState.User);
        }
    }
}
