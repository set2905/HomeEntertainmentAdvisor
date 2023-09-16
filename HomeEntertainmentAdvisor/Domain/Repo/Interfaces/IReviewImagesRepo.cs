using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IReviewImagesRepo : IRepo<ReviewImage, Guid>
    {
        Task<string?> GetFirstImageUrl(Guid reviewId);
        Task<List<ReviewImage>> GetImagesForReview(Guid reviewId);
    }
}
