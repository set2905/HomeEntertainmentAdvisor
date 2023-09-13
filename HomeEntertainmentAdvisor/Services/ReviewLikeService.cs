using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HomeEntertainmentAdvisor.Services
{
    public class ReviewLikeService : IReviewLikeService
    {
        private const int LIKECOUNT_UPDATE_SECONDS = 3600;

        private readonly IReviewLikesRepo reviewLikesRepo;
        private readonly IReviewService reviewService;

        public ReviewLikeService(IReviewLikesRepo reviewLikesRepo, IReviewService reviewService)
        {
            this.reviewService = reviewService;
            this.reviewLikesRepo= reviewLikesRepo;
        }
        public async Task<bool> IsLikedByUser(Review review, string userId)
        {
            ReviewLike? like = await reviewLikesRepo.GetById((review.Id, userId));
            if (like == null) return false;
            return true;
        }
        public async Task<int> UpdateLikeCount(Review review)
        {

            if (review.LastCacheUpdate < DateTime.Now-TimeSpan.FromSeconds(3600))
            {
                int likeCount = await reviewLikesRepo.GetLikeCount(review.Id);
                review.LastCacheUpdate = DateTime.Now;
                review.CachedLikes = likeCount;
                await reviewService.TrySaveReview(review);
                return likeCount;
            }
            else
                return review.CachedLikes;
        }
        public async Task<bool> LikeReview(Guid reviewId, string userId, CancellationToken cancellationToken = default)
        {
            Review? review = await reviewService.GetById(reviewId);
            if (review == null) return false;
            ReviewLike? like = await reviewLikesRepo.GetById((reviewId, userId));
            if (like != null) return false;
            await reviewLikesRepo.Save(new() { UserId=userId, ReviewId=reviewId }, cancellationToken);
            return true;

        }
        public async Task<bool> RemoveLikeReview(Guid reviewId, string userId, CancellationToken cancellationToken = default)
        {
            Review? review = await reviewService.GetById(reviewId);
            if (review == null) return false;
            ReviewLike? like = await reviewLikesRepo.GetById((reviewId, userId));
            if (like == null) return false;
            return await reviewLikesRepo.Delete(like, cancellationToken);
        }

    }
}
