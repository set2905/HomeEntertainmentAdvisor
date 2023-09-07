using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IReviewImagesRepo : IRepo<ReviewImage, Guid>
    {
        Task<List<ReviewImage>> GetImagesForReview(Guid reviewId);
    }
}
