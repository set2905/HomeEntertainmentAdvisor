using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IRatingRepo : IRepo<Rating, (string,Guid)>
    {
        Task<double> GetAvgMediaRating(Guid mediaPieceId);
        Task<Rating?> GetRating(string userId, Guid mediaId);
    }
}
