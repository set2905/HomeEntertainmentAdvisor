using HomeEntertainmentAdvisor.Domain.Repo;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Pages;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace HomeEntertainmentAdvisor.Services
{
    public class ReviewService : AuthServiceBase, IReviewService
    {
        private readonly IReviewsRepo reviewsRepo;
        private readonly IRatingRepo ratingRepo;
        private readonly IStringLocalizer<EditReview> localizer;

        public ReviewService(IStringLocalizer<EditReview> localizer,IReviewsRepo reviewsRepo, IRatingRepo ratingRepo, AuthenticationStateProvider authenticationStateProvider, UserManager<User> userManager, IAuthorizationService authorizationService) : base(authenticationStateProvider, userManager, authorizationService)
        {
            this.reviewsRepo = reviewsRepo;
            this.ratingRepo=ratingRepo;
            this.localizer = localizer;
        }
        /// <summary>
        /// Sets status for review
        /// </summary>
        /// <param name="review"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<bool> SetStatus(Review review, ReviewStatus status)
        {
            if (!await IsUserNotBlocked()) return false;
            await reviewsRepo.SetStatus(review, status);
            return true;
        }
        /// <summary>
        /// Gets all reviews, that belong to current user
        /// </summary>
        /// <returns></returns>
        public async Task<List<Review>> GetMyReviews()
        {
            User? user = await GetUser(await GetAuthState());
            if (user == null) return new();
            return await GetUserReviews(user.Id);
        }
        /// <summary>
        /// Gets all reviews, that belong to user with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Review>> GetUserReviews(string id)
        {
            return await reviewsRepo.GetUserReviews(id);
        }
        /// <summary>
        /// Gets review by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Review?> GetById(Guid id)
        {
            return await reviewsRepo.GetById(id);
        }
        /// <summary>
        /// Gets reviews by page number, page size, search query, tag filter, sort order
        /// </summary>
        /// <param name="page"></param>
        /// <param name="recordsPerPage"></param>
        /// <param name="searchQuery"></param>
        /// <param name="tags"></param>
        /// <param name="order"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Review>> GetPage(int page,
                                                int recordsPerPage,
                                                string? searchQuery = null,
                                                IEnumerable<Tag>? tags = null,
                                                ReviewOrder order = ReviewOrder.Date,
                                                CancellationToken cancellationToken = default)
        {
            return await reviewsRepo.GetPage(page, recordsPerPage, searchQuery, tags, order, cancellationToken);
        }

        /// <summary>
        /// Makes an attempt to save review
        /// </summary>
        /// <param name="review"></param>
        /// <returns>Tuple, containing: id of saved review; bool, indicating success; message of save result</returns>
        public async Task<(Guid id, bool succeeded, string message)> TrySaveReview(Review review)
        {
            AuthenticationState authState = await GetAuthState();
            User? user = await GetUser(authState);
            if (user==null)
            {
                return (Guid.Empty, false, localizer["usernotfound"]);
            }
            Rating? existingRating = await ratingRepo.GetById((user.Id, review.Rating.MediaPiece.Id));
            if (existingRating!=null && review.Id==default && await reviewsRepo.GetByRating(existingRating)!=null)
            {
                return (Guid.Empty, false, $"{localizer["reviewexists"]} {review.Rating.MediaPiece.Name}");
            }
            if (!TrySetReviewAuthor(user, review))
            {
                if (!await IsUserAuthor(authState, review) && !await IsUserAdmin(user))
                {
                    return (Guid.Empty, false, localizer["notowner"]);
                }
            }
            var ratingId = await ratingRepo.Save(review.Rating);
            review.RatingAuthorId=ratingId.Item1;
            review.RatingMediaPieceId=ratingId.Item2;
            return (await reviewsRepo.Save(review), true, localizer["saved"]);
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
