using HomeEntertainmentAdvisor.Domain.Repo;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;


namespace HomeEntertainmentAdvisor.Services
{
    public class ReviewService : AuthServiceBase, IReviewService
    {
        private readonly IReviewsRepo reviewsRepo;
        private readonly IRatingRepo ratingRepo;

        public ReviewService(IReviewsRepo reviewsRepo, IRatingRepo ratingRepo, AuthenticationStateProvider authenticationStateProvider, UserManager<User> userManager, IAuthorizationService authorizationService) : base(authenticationStateProvider, userManager, authorizationService)
        {
            this.reviewsRepo = reviewsRepo;
            this.ratingRepo=ratingRepo;
        }
        public async Task SetStatus(Review review, ReviewStatus status)
        {
            await reviewsRepo.SetStatus(review, status);
        }
        public async Task SetStatus(IEnumerable<Review> reviews, ReviewStatus status)
        {
            foreach (Review review in reviews)
            {
                await SetStatus(review, status);
            }
        }
        public async Task<List<Review>> GetMyReviews()
        {
            User? user = await GetUser(await GetAuthState());
            if (user == null) return new();
            return await GetUserReviews(user.Id);
        }
        public async Task<List<Review>> GetUserReviews(string id)
        {
            return await reviewsRepo.GetUserReviews(id);
        }
        public async Task<Review?> GetById(Guid id)
        {
            return await reviewsRepo.GetById(id);
        }
        public async Task<List<Review>> GetPage(int page,
                                                int recordsPerPage,
                                                string? searchQuery = null,
                                                IEnumerable<Tag>? tags = null,
                                                ReviewOrder order = ReviewOrder.Date)
        {
            return await reviewsRepo.GetPage(page, recordsPerPage, searchQuery, tags, order);
        }



        public async Task<(Guid id, bool succeeded, string message)> TrySaveReview(Review review)
        {
            AuthenticationState authState = await GetAuthState();
            User? user = await GetUser(authState);
            if (user==null)
            {
                return (Guid.Empty, false, "User could not be found!");
            }
            if (!TrySetReviewAuthor(user, review))
            {
                if (!await IsUserAuthor(authState, review) && !await IsUserAdmin(user))
                {
                    return (Guid.Empty, false, "You are not the owner of this resource");
                }
            }
            Guid ratingId = await ratingRepo.Save(review.Rating);
            review.RatingId=ratingId;
            return (await reviewsRepo.Save(review), true, "Review saved");
        }

        private bool TrySetReviewAuthor(User user, Review review)
        {
            if (review.Id == default && (review.Rating.AuthorId == default || review.Rating.Author == null))
            {
                review.Rating.Author = user;
                review.Rating.AuthorId = user.Id;
                return true;
            }
            else return false;
        }
    }
}
