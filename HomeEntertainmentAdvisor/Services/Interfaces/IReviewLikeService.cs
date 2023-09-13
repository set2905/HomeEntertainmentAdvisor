using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IReviewLikeService
    {
        Task<int> UpdateLikeCount(Review review);
        Task<bool> LikeReview(Guid reviewId, string userId, CancellationToken cancellationToken = default);
        Task<bool> RemoveLikeReview(Guid reviewId, string userId, CancellationToken cancellationToken = default);
        Task<bool> IsLikedByUser(Review review, string userId);
    }
}