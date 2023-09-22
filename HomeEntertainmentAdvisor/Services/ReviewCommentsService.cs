using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HomeEntertainmentAdvisor.Services
{
    public class ReviewCommentsService : AuthServiceBase, IReviewCommentsService
    {
        private readonly ICommentsRepo commentsRepo;

        public ReviewCommentsService(ICommentsRepo commentsRepo, AuthenticationStateProvider authenticationStateProvider, UserManager<User> userManager, IAuthorizationService authorizationService) : base(authenticationStateProvider, userManager, authorizationService)
        {
            this.commentsRepo=commentsRepo;
        }
        /// <summary>
        /// Gets comment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Comment?> GetById(Guid id)
        {
            return await commentsRepo.GetById(id);
        }
        /// <summary>
        /// Gets comments by page number and size on a review with specified id
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<Comment>> GetPage(Guid reviewId, int page = 0, int pageSize = 10)
        {
            return await commentsRepo.GetComments(reviewId, page*pageSize, pageSize);
        }
        /// <summary>
        /// Adds a new comment to review
        /// </summary>
        /// <param name="content"></param>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public async Task<Guid> AddComment(string content, Guid reviewId)
        {
            User? user = await GetUser(await GetAuthState());
            if (user==null) return default;
            Comment comment = new()
            {
                AuthorId=user.Id,
                ReviewId=reviewId,
                Content=content,
                CreatedDate=DateTime.Now,
            };
            return await commentsRepo.Save(comment);
        }
    }
}
