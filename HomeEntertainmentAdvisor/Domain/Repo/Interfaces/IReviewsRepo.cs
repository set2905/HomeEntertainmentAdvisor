using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IReviewsRepo : IRepo<Review, Guid>
    {
        Task<List<Review>> GetPage(int page, int recordsPerPage, string searhQuery);
        Task<List<Review>> GetPage(int page, int recordsPerPage);
        Task<List<Review>> GetUserReviews(string userId);
        Task<bool> SetStatus(Review entity, ReviewStatus status);
    }
}