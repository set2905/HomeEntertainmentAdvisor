using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IRatingRepo : IRepo<Rating, Guid>
    {
        Task<double> GetAvgMediaRating(Guid mediaPieceId);
        Task<Rating?> GetByMediaAndUser(string userId, Guid mediaId);
    }
}
