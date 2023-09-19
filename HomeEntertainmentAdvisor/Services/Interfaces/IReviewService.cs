using HomeEntertainmentAdvisor.Domain.Repo;
using HomeEntertainmentAdvisor.Models;
using MudBlazor;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IReviewService
    {
        Task<Review?> GetById(Guid id);
        Task<(Guid id, bool succeeded, string message)> TrySaveReview(Review review);
        Task<List<Review>> GetMyReviews();
        Task<List<Review>> GetUserReviews(string id);
        Task<List<Review>> GetPage(int page, int recordsPerPage, string? searchQuery = null, IEnumerable<Tag>? tags = null, ReviewOrder order = ReviewOrder.Date, CancellationToken cancellationToken = default);
        Task<bool> SetStatus(Review review, ReviewStatus status);
    }
}
