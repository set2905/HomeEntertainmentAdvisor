using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class RatingRepo : RepoBase<Rating, Guid>, IRatingRepo
    {
        public RatingRepo(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public override async Task<Rating?> GetById(Guid id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Rating>();
                return await dbSet.SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public override async Task<Guid> Save(Rating entity, CancellationToken cancellationToken = default)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Rating>();
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
