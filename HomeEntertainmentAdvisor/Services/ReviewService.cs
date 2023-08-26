using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services
{
    public class ReviewService
    {
        private readonly IReviewsRepo reviewsRepo;
        private readonly IRatingRepo ratingRepo;

        public async Task<List<Review>> GetNewest()
        {
            return await reviewsRepo.GetPage(1, 10);
        }
    }
}
