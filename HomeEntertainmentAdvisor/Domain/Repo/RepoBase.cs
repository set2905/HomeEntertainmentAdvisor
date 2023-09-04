using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public abstract class RepoBase<TEntity, TId> : IRepo<TEntity, TId> where TEntity : class
    {
        protected readonly IDbContextFactory<ApplicationDbContext> contextFactory;

        protected RepoBase(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            this.contextFactory=contextFactory;
        }

        public virtual async Task<bool> Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
        /// <inheritdoc />
        public virtual async Task<List<TEntity>> GetAll()
        {
            using (var context = contextFactory.CreateDbContext())
            {
                return await context.Set<TEntity>().ToListAsync();
            }
        }

        public abstract Task<TEntity?> GetById(TId id);

        public abstract Task<TId> Save(TEntity entity, CancellationToken cancellationToken = default);
    }
}
