using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class MediaPieceGroupsRepo : RepoBase<MediaGroup, int>, IMediaPieceGroupsRepo
    {
        private const int EMPTYSEARCH_COUNT = 10;

        public MediaPieceGroupsRepo(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }
        public async Task<List<MediaGroup>> Search(string value, CancellationToken cancellationToken = default)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<MediaGroup>();
                if (string.IsNullOrEmpty(value))
                    return await dbSet.OrderBy(x => x.Name).Take(EMPTYSEARCH_COUNT).ToListAsync();
                var result = await dbSet.Where(x => x.Name.StartsWith(value)).ToListAsync(cancellationToken);
                return result;
            }
        }
        public override async Task<MediaGroup?> GetById(int id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<MediaGroup>();
                return await dbSet.SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public override async Task<int> Save(MediaGroup entity, CancellationToken cancellationToken = default)
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
