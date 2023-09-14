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
        private readonly IReviewsRepo reviewRepo;
        private readonly UserManager<User> userManager;
        public ReviewLikeService(IReviewLikesRepo reviewLikesRepo, IReviewsRepo reviewRepo, UserManager<User> userManager)
        {
            this.reviewRepo = reviewRepo;
            this.reviewLikesRepo= reviewLikesRepo;
            this.userManager = userManager;
        }
        public async Task<User?> TryUpdateUserLikes(string userId)
        {
            User? user = await userManager.FindByIdAsync(userId);
            if (user == null) return null;
            if (user.LastCacheUpdate < DateTime.Now-TimeSpan.FromSeconds(LIKECOUNT_UPDATE_SECONDS))
            {
                int likeCount = await reviewLikesRepo.GetUserLikesCount(user.Id);
                user.LastCacheUpdate = DateTime.Now;
                user.CachedLikes = likeCount;
                await userManager.UpdateAsync(user);
                return user;
            }
            else
                return user;
        }
        public async Task<bool> IsLikedByUser(Review review, string userId)
        {
            ReviewLike? like = await reviewLikesRepo.GetById((review.Id, userId));
            if (like == null) return false;
            return true;
        }
        public async Task<int> TryUpdateReviewLikeCount(Review review)
        {
            if (review.LastCacheUpdate < DateTime.Now-TimeSpan.FromSeconds(LIKECOUNT_UPDATE_SECONDS))
            {
                int likeCount = await reviewLikesRepo.GetLikeCount(review.Id);
                review.LastCacheUpdate = DateTime.Now;
                review.CachedLikes = likeCount;
                await reviewRepo.Save(review);
                return likeCount;
            }
            else
                return review.CachedLikes;
        }
        public async Task<bool> LikeReview(Guid reviewId, string userId, CancellationToken cancellationToken = default)
        {
            Review? review = await reviewRepo.GetById(reviewId);
            if (review == null) return false;
            ReviewLike? like = await reviewLikesRepo.GetById((reviewId, userId));
            if (like != null) return false;
            await reviewLikesRepo.Save(new() { UserId=userId, ReviewId=reviewId }, cancellationToken);
            return true;

        }
        public async Task<bool> RemoveLikeReview(Guid reviewId, string userId, CancellationToken cancellationToken = default)
        {
            Review? review = await reviewRepo.GetById(reviewId);
            if (review == null) return false;
            ReviewLike? like = await reviewLikesRepo.GetById((reviewId, userId));
            if (like == null) return false;
            return await reviewLikesRepo.Delete(like, cancellationToken);
        }

    }
}
