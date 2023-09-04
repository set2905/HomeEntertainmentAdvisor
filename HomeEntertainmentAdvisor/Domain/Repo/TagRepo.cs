using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class TagRepo : RepoBase<Tag, Guid>, ITagRepo
    {
        public TagRepo(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<Tag?> GetByName(string name)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Tag>();
                return await context.Tags.SingleOrDefaultAsync(x => x.Name == name);
            }
        }

        public async Task<List<Tag>> SearchByName(string query, CancellationToken cancellationToken = default)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Tag>();
                return await dbSet.Where(x => x.Name.StartsWith(query)).ToListAsync(cancellationToken);
            }
        }
        public override async Task<Tag?> GetById(Guid id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Tag>();
                return await dbSet.SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public override async Task<Guid> Save(Tag entity)
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
