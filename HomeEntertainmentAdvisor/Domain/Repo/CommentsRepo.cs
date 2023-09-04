using HomeEntertainmentAdvisor.Data;
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

        public override async Task<Comment?> GetById(Guid id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                return await context.Set<Comment>().SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public override async Task<Guid> Save(Comment entity)
        {
            using (var context = contextFactory.CreateDbContext())
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
}
