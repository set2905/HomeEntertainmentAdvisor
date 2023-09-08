using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using MailKit.Search;
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
        public async Task<List<Review>> GetPage(int page, int recordsPerPage, IEnumerable<Tag> tags)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                return await context.ReviewTagRelations.Include(x => x.Tag)
                    .Where(x => tags.Contains(x.Tag))
                    .Select(x => x.Review)
                    .Distinct()
                    .OrderBy(x => x.CreatedDate)
                    .Skip(page*recordsPerPage)
                    .Take(recordsPerPage)
                    .ToListAsync();
            }
        }

        public async Task<List<Review>> GetPage(int page, int recordsPerPage)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Review>();
                return await dbSet.Where(x => x.Status==ReviewStatus.Published)
                    .OrderBy(x => x.CreatedDate)
                    .Skip(page*recordsPerPage)
                    .Take(recordsPerPage)
                    .Include(r => r.Rating)
                    .ThenInclude(r => r.Author)
                    .ToListAsync();
            }
        }

        public async Task<List<Review>> GetPage(int page, int recordsPerPage, string searhQuery)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Review>();
                var foundInReviewsIds = dbSet.Where(x => EF.Functions.FreeText(x.Content, searhQuery)).Select(x => x.Id);
                var foundInCommentsIds = context.Comments.Where(x => EF.Functions.FreeText(x.Content, searhQuery)).Select(x => x.ReviewId);
                var foundByTagIds = GetSearchByTagQuery(context, searhQuery);
                var foundIds = foundInReviewsIds.Union(foundInCommentsIds).Union(foundByTagIds).Distinct();
                IQueryable<Review> found = dbSet.Where(x => foundIds.Contains(x.Id)&&x.Status==ReviewStatus.Published);
                return await found.OrderBy(x => x.CreatedDate)
                    .Skip(page*recordsPerPage)
                    .Take(recordsPerPage)
                    .Include(r => r.Rating)
                    .ThenInclude(r => r.Author)
                    .ToListAsync();
            }
        }

        private IQueryable<Guid> GetSearchByTagQuery(ApplicationDbContext context, string searchQuery)
        {
            var foundTags = context.Tags.Where(x => x.Name.StartsWith(searchQuery));
            return context.ReviewTagRelations.Where(x => foundTags.Select(t => t.Id).Contains(x.TagId)).Select(x => x.ReviewId);
        }
        public override async Task<Review?> GetById(Guid id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Review>();
                return await context.Reviews.Include(x => x.Rating).ThenInclude(x => x.MediaPiece).SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public override async Task<Guid> Save(Review entity, CancellationToken cancellationToken = default)
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
