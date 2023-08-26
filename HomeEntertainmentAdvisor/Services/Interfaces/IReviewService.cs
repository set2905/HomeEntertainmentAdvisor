using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IReviewService
    {
        Task CreateReview();
        Task<List<Review>> Find( string query, int page=0, int perPage=10);
        Task<List<Review>> GetNewest();
    }
}
