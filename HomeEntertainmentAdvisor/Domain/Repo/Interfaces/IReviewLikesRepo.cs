using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IReviewLikesRepo : IRepo<ReviewLike, (Guid, string)>
    {
    }
}
