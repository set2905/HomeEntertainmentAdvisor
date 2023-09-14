using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IReviewLikesRepo : IRepo<ReviewLike, (Guid, string)>
    {
        Task<int> GetLikeCount(Guid reviewId);
        Task<int> GetUserLikesCount(string userId);
    }
}
