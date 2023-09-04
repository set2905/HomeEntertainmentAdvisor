using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class MediaPiecesRepo : RepoBase<MediaPiece, Guid>, IMediaPiecesRepo
    {
        public MediaPiecesRepo(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public override async Task<MediaPiece?> GetById(Guid id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<MediaPiece>();
                return await dbSet.SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public override async Task<Guid> Save(MediaPiece entity, CancellationToken cancellationToken = default)
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
        public async Task<List<MediaPiece>> Search(string value, CancellationToken cancellationToken = default)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<MediaPiece>();
                if (string.IsNullOrEmpty(value))
                    return await dbSet.OrderBy(x => x.Name).Take(10).ToListAsync();
                var result = await dbSet.Where(x => x.Name.StartsWith(value)).ToListAsync(cancellationToken);
                return result;
            }
        }
    }
}
