﻿using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class ReviewsRepo : RepoBase<Review, Guid>, IReviewsRepo
    {
        public ReviewsRepo(ApplicationDbContext context) : base(context)
        {
            dbSet=context.Reviews;
        }

        public async Task<List<Review>> GetPage(int page, int recordsPerPage)
        {
            //if (page<1) throw new ArgumentException("page cannot be less than 1");
            //if (recordsPerPage<1) throw new ArgumentException("recordsPerPage cannot be less than 1");
            return await dbSet.OrderBy(x => x.CreatedDate).Skip((page-1)*recordsPerPage).Take(recordsPerPage).ToListAsync();
        }

        public async Task<List<Review>> GetPage(int page, int recordsPerPage, string searhQuery)
        {
            var foundIdsInReviews = dbSet.Where(x => EF.Functions.FreeText(x.Content, searhQuery)).Select(x => x.Id);
            var foundIdsInComments = context.Comments.Where(x => EF.Functions.FreeText(x.Content, searhQuery)).Select(x => x.ReviewId);
            var foundIds = foundIdsInReviews.Union(foundIdsInComments).Distinct();
            IQueryable<Review> found = dbSet.Where(x => foundIds.Contains(x.Id)).Include(r => r.Rating).ThenInclude(r => r.Author);
            return await found.OrderBy(x => x.CreatedDate).Skip(page*recordsPerPage).Take(recordsPerPage).ToListAsync();
        }
        public override async Task<Review?> GetById(Guid id)
        {
            return await context.Reviews.SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<Guid> Save(Review entity)
        {
            if (entity.Id == default)
                context.Entry(entity).State = EntityState.Added;
            else
                context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity.Id;
        }
    }
}
