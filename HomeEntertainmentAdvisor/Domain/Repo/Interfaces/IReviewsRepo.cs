using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IReviewsRepo : IRepo<Review, Guid>
    {
        Task<Review?> GetByRating(Rating rating);
        Task<List<Review>> GetPage(int page, int recordsPerPage, string? searchQuery = null, IEnumerable<Tag>? tags = null, ReviewOrder order = ReviewOrder.Date, CancellationToken cancellationToken = default);

        Task<List<Review>> GetUserReviews(string userId);
        Task<bool> SetStatus(Review entity, ReviewStatus status);
    }
}