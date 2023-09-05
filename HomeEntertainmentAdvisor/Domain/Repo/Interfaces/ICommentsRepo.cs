using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface ICommentsRepo : IRepo<Comment, Guid>
    {
        Task<List<Comment>> GetComments(Guid reviewId, int skip = 0, int take = 10);
    }
}
