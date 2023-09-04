using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class ReviewsRepo : RepoBase<Review, Guid>, IReviewsRepo
    {
        public ReviewsRepo(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<List<Review>> GetUserReviews(string userId)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Review>();
                return await dbSet.Include(r => r.Rating).Where(x => x.Rating.AuthorId==userId).ToListAsync();
            }
        }
        public async Task<List<Review>> GetPage(int page, int recordsPerPage)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Review>();
                return await dbSet.Where(x => x.Status==ReviewStatus.Published).OrderBy(x => x.CreatedDate).Skip((page-1)*recordsPerPage).Take(recordsPerPage).Include(r => r.Rating).ThenInclude(r => r.Author).ToListAsync();
            }
        }

        public async Task<List<Review>> GetPage(int page, int recordsPerPage, string searhQuery)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Review>();
                var foundIdsInReviews = dbSet.Where(x => EF.Functions.FreeText(x.Content, searhQuery)).Select(x => x.Id);
                var foundIdsInComments = context.Comments.Where(x => EF.Functions.FreeText(x.Content, searhQuery)).Select(x => x.ReviewId);
                var foundIds = foundIdsInReviews.Union(foundIdsInComments).Distinct();
                IQueryable<Review> found = dbSet.Where(x => foundIds.Contains(x.Id)&&x.Status==ReviewStatus.Published);
                return await found.OrderBy(x => x.CreatedDate).Skip(page*recordsPerPage).Take(recordsPerPage).Include(r => r.Rating).ThenInclude(r => r.Author).ToListAsync();
            }
        }
        public override async Task<Review?> GetById(Guid id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Review>();
                return await context.Reviews.Include(x => x.Rating).ThenInclude(x => x.MediaPiece).SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public override async Task<Guid> Save(Review entity)
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
        public async Task<bool> SetStatus(Review entity, ReviewStatus status)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                if (entity.Status==status)
                {
                    return false;
                }
                entity.Status=status;
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}
