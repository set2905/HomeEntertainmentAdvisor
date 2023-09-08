using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IReviewTagRelationsRepo : IRepo<ReviewTagRelation, (Guid, Guid)>
    {
        Task<List<Tag>> GetTagsByReviewId(Guid reviewId, int take = 0);
        Task<bool> RemoveByReviewId(Guid reviewId);
    }
}
