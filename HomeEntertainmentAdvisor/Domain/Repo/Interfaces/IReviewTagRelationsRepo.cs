using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IReviewTagRelationsRepo : IRepo<ReviewTagRelation, (Guid, Guid)>
    {
        Task<List<Tag>> GetTagsByReviewId(Guid reviewId);
        Task<bool> RemoveByReviewId(Guid reviewId);
    }
}
