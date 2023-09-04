using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IReviewLikeService
    {
        Task<int> UpdateLikeCount(Review review);
        Task<bool> LikeReview(Guid reviewId);
        Task<bool> RemoveLikeReview(Guid reviewId);
        Task<bool> IsLikedByUser(Review review);
    }
}