using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class ReviewLikesRepo : RepoBase<ReviewLike, (Guid, string)>, IReviewLikesRepo
    {
        public ReviewLikesRepo(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<int> GetLikeCount(Guid reviewId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<ReviewLike>();
                return await dbSet.Where(x => x.ReviewId==reviewId).CountAsync();
            }
        }
        public async Task<int> GetUserLikesCount(string userId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<ReviewLike>();
                return await dbSet.Include(x=>x.Review).ThenInclude(x=>x.Rating).Where(x => x.Review.Rating.AuthorId==userId).CountAsync();
            }
        }
        public override async Task<ReviewLike?> GetById((Guid, string) id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<ReviewLike>();
                return await dbSet.SingleOrDefaultAsync(x => x.ReviewId == id.Item1&&x.UserId==id.Item2);
            }

        }

        public override async Task<(Guid, string)> Save(ReviewLike entity, CancellationToken cancellationToken = default)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                context.Entry(entity).State = EntityState.Added;
                await context.SaveChangesAsync(cancellationToken);
                return (entity.ReviewId, entity.UserId);
            }
        }
    }
}
