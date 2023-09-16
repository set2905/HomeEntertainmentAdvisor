using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IRatingService
    {
        Task<Rating?> GetById((string, Guid) id);
        Task<Rating?> GetByMedia(MediaPiece media, string user);
        Task<(string, Guid)> SaveRating(Rating rating, string user);
        Task<double> TryUpdateMediaRating(MediaPiece media);
    }
}
