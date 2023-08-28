using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IReviewService
    {
        Task EditReview();
        Task<List<Review>> Find( string query, int page=0, int perPage=10);
        Task<List<Review>> GetNewest();
    }
}
