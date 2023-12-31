﻿using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class ReviewTagRelationsRepo : RepoBase<ReviewTagRelation, (Guid, Guid)>, IReviewTagRelationsRepo
    {
        public ReviewTagRelationsRepo(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<List<Tag>> GetTagsByReviewId(Guid reviewId, int take = 0)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<ReviewTagRelation>();
                var reviewTagsQuery = dbSet.Where(x => x.ReviewId==reviewId);
                if (take>0 && take<await reviewTagsQuery.CountAsync())
                    reviewTagsQuery=reviewTagsQuery.Take(take);
                return await reviewTagsQuery.Include(x => x.Tag).Select(x => x.Tag).ToListAsync();
            }
        }
        public async Task<bool> RemoveByReviewId(Guid reviewId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<ReviewTagRelation>();
                List<ReviewTagRelation> found = await dbSet.Where(x => x.ReviewId==reviewId).Include(x => x.Tag).ToListAsync();
                if (found.Count()==0) return false;
                foreach (ReviewTagRelation r in found)
                {
                    await Delete(r);
                    r.Tag.Popularity-=1;
                    await context.SaveChangesAsync();
                }
                return true;
            }
        }
        public override async Task<ReviewTagRelation?> GetById((Guid, Guid) id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<ReviewTagRelation>();
                return await dbSet.SingleOrDefaultAsync(x => x.ReviewId == id.Item1&&x.TagId==id.Item2);
            }

        }

        public override async Task<(Guid, Guid)> Save(ReviewTagRelation entity, CancellationToken cancellationToken = default)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<ReviewTagRelation>();
                context.Entry(entity).State = EntityState.Added;
                await context.SaveChangesAsync(cancellationToken);
                return (entity.ReviewId, entity.TagId);
            }
        }
    }
}
