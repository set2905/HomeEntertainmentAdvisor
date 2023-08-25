using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public abstract class RepoBase<TEntity, TId> : IRepo<TEntity, TId> where TEntity : class
    {
        protected readonly ApplicationDbContext context;
        protected DbSet<TEntity> dbSet;

        public RepoBase(ApplicationDbContext context)
        {
            this.context = context;
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }
        /// <inheritdoc />
        public virtual async Task<List<TEntity>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public abstract Task<TEntity?> GetById(TId id);

        public abstract Task<TId> Save(TEntity entity);
    }
}
