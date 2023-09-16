using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class RatingRepo : RepoBase<Rating, (string, Guid)>, IRatingRepo
    {
        public RatingRepo(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public override async Task<Rating?> GetById((string, Guid) id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Rating>();
                return await dbSet.SingleOrDefaultAsync(x => x.AuthorId==id.Item1 && x.MediaPieceId == id.Item2);
            }
        }
        public async Task<double> GetAvgMediaRating(Guid mediaPieceId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Rating>();
                return await dbSet.Where(x => x.MediaPieceId== mediaPieceId).AverageAsync(x => x.Grade);
            }
        }

        public async Task<Rating?> GetRating(string userId, Guid mediaId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Rating>();
                return await dbSet.FirstOrDefaultAsync(x => x.AuthorId==userId&&x.MediaPieceId==mediaId);
            }
        }

        public override async Task<(string, Guid)> Save(Rating entity, CancellationToken cancellationToken = default)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Rating>();
                if (entity.AuthorId==null) return default;
                if (await GetById((entity.AuthorId, entity.MediaPieceId)) == null)
                    context.Entry(entity).State = EntityState.Added;
                else
                    context.Entry(entity).State = EntityState.Modified;
                await context.SaveChangesAsync(cancellationToken);
                return (entity.AuthorId, entity.MediaPieceId);
            }
        }
    }
}
