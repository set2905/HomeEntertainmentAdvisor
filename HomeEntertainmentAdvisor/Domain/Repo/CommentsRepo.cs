﻿using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class CommentsRepo : RepoBase<Comment, Guid>, ICommentsRepo
    {
        public CommentsRepo(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<List<Comment>> GetComments(Guid reviewId, int skip = 0, int take = 10)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                return await context.Set<Comment>().Where(x => x.ReviewId==reviewId).OrderByDescending(x => x.CreatedDate).Skip(skip).Take(take).Include(x => x.Author).ToListAsync();
            }
        }
        public override async Task<Comment?> GetById(Guid id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                return await context.Set<Comment>().Include(x => x.Author).SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public override async Task<Guid> Save(Comment entity, CancellationToken cancellationToken = default)
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
