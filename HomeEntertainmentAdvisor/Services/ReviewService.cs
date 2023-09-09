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

        public ReviewService(IReviewsRepo reviewsRepo, AuthenticationStateProvider authenticationStateProvider, UserManager<User> userManager, IAuthorizationService authorizationService) : base(authenticationStateProvider, userManager, authorizationService)
        {
            this.reviewsRepo = reviewsRepo;
        }

        public async Task SetStatus(IEnumerable<Review> toDelete, ReviewStatus status)
        {
            foreach (Review review in toDelete)
            {
                await reviewsRepo.SetStatus(review, status);
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
        public async Task<List<Review>> GetOrdered(int page = 0, int recordsPerPage = 10, ReviewOrder order = ReviewOrder.Date)
        {
            return await reviewsRepo.GetPage(page, recordsPerPage, order);
        }
        public async Task<List<Review>> Search(string query, int page = 0, int perPage = 10)
        {
            return await reviewsRepo.GetPage(page, perPage, query);
        }
        public async Task<List<Review>> SearchByTags(int page, int recordsPerPage, IEnumerable<Tag> tags)
        {
            return await reviewsRepo.GetPage(page, recordsPerPage, tags);
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
