using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IReviewTagRelationsRepo : IRepo<ReviewTagRelation, (Guid,Guid)>
    {
    }
}
