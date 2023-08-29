using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IReviewService
    {
        Task<Guid> SaveReview(Review review);
        Task<List<Review>> FindReviews(string query, int page = 0, int perPage = 10);
        Task<List<Review>> GetNewest();
    }
}
