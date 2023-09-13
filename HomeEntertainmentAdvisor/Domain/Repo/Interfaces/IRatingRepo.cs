using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IRatingRepo : IRepo<Rating, Guid>
    {
        Task<Rating?> GetByMedia(string userId, Guid mediaId);
    }
}
