﻿using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
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
        public async Task<Comment?> GetById(Guid id)
        {
            return await commentsRepo.GetById(id);
        }
        public async Task<List<Comment>> GetPage(Guid reviewId, int page = 0, int pageSize = 10)
        {
            return await commentsRepo.GetComments(reviewId, page*pageSize, pageSize);
        }
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
