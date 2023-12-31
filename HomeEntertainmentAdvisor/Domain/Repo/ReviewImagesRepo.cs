﻿using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class ReviewImagesRepo : RepoBase<ReviewImage, Guid>, IReviewImagesRepo
    {
        public ReviewImagesRepo(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public override async Task<ReviewImage?> GetById(Guid id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<ReviewImage>();
                return await dbSet.SingleOrDefaultAsync(x => x.Id == id);
            }
        }
        public async Task<List<ReviewImage>> GetImagesForReview(Guid reviewId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<ReviewImage>();
                return await dbSet.Where(x => x.ReviewId==reviewId).ToListAsync();
            }
        }
        public async Task<string?> GetFirstImageUrl(Guid reviewId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<ReviewImage>();
                return await dbSet.Where(x => x.ReviewId==reviewId).Select(x => x.Url).FirstOrDefaultAsync();
            }
        }
        public override async Task<Guid> Save(ReviewImage entity, CancellationToken cancellationToken = default)
        {
            using (var context = contextFactory.CreateDbContext())
            {

                if (entity.Id == default)
                    context.Entry(entity).State = EntityState.Added;
                else
                    context.Entry(entity).State = EntityState.Modified;
                await context.SaveChangesAsync(cancellationToken);
                return entity.Id;
            }
        }
    }
}
