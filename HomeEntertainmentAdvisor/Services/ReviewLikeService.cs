using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HomeEntertainmentAdvisor.Services
{
    public class ReviewLikeService : AuthServiceBase, IReviewLikeService
    {

        private readonly IReviewLikesRepo reviewLikesRepo;
        private readonly IReviewService reviewService;

        public ReviewLikeService(IReviewLikesRepo reviewLikesRepo, IReviewService reviewService, AuthenticationStateProvider authenticationStateProvider, UserManager<User> userManager, IAuthorizationService authorizationService) : base(authenticationStateProvider, userManager, authorizationService)
        {
            this.reviewService = reviewService;
            this.reviewLikesRepo= reviewLikesRepo;
        }

        public async Task<bool> LikeReview(Guid reviewId)
        {
            User? user = await GetUser(await GetAuthState());
            if (user == null) return false;
            Review? review = await reviewService.GetById(reviewId);
            if (review == null) return false;
            ReviewLike? like = await reviewLikesRepo.GetById((reviewId, user.Id));
            if (like != null) return false;
            await reviewLikesRepo.Save(new() { UserId=user.Id, ReviewId=reviewId });
            return true;

        }
        public async Task<bool> RemoveLikeReview(Guid reviewId)
        {
            User? user = await GetUser(await GetAuthState());
            if (user == null) return false;
            Review? review = await reviewService.GetById(reviewId);
            if (review == null) return false;
            ReviewLike? like = await reviewLikesRepo.GetById((reviewId, user.Id));
            if (like == null) return false;
            return await reviewLikesRepo.Delete(like);
        }

    }
}
