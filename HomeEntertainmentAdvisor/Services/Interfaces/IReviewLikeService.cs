namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IReviewLikeService
    {
        Task<int> GetLikeCount(Guid reviewId);
        Task<bool> LikeReview(Guid reviewId);
        Task<bool> RemoveLikeReview(Guid reviewId);
    }
}