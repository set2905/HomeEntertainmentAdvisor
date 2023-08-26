using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;

namespace HomeEntertainmentAdvisor.Services
{
    public class ReviewService
    {
        private readonly IReviewsRepo reviewsRepo;
        private readonly IRatingRepo ratingRepo;
        private readonly IReviewTagRelationsRepo reviewTagRelationsRepo;
    }
}
