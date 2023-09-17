using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public enum ReviewOrder
    {
        Date,
        Likes
    }
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
        public async Task<Review?> GetByRating(Rating rating)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Review>();
                return await dbSet.Where(x => x.RatingMediaPieceId==rating.MediaPieceId&&x.RatingAuthorId==rating.AuthorId).FirstOrDefaultAsync();
            }
        }

        public async Task<List<Review>> GetPage(int page,
                                                int recordsPerPage,
                                                string? searchQuery = null,
                                                IEnumerable<Tag>? tags = null,
                                                ReviewOrder order = ReviewOrder.Date,
                                                CancellationToken cancellationToken = default)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Review>();
                IQueryable<Review> result = dbSet;
                if (searchQuery!=null)
                {
                    IQueryable<Guid> foundIds = GetSearchQuery(context, searchQuery);
                    result = result.Where(x => foundIds.Contains(x.Id));
                }
                if (!tags.IsNullOrEmpty())
                {
                    result = result.Intersect(context.ReviewTagRelations
                    .Include(x => x.Tag)
                    .Where(x => tags.Contains(x.Tag))
                    .Select(x => x.Review)
                    .Distinct());
                }
                return await GetPageQuery(result, page, recordsPerPage, context, order).ToListAsync(cancellationToken);
            }
        }
        private IQueryable<Review> GetPageQuery(IQueryable<Review> sourceQuery,
                                                int page,
                                                int recordsPerPage,
                                                ApplicationDbContext context,
                                                ReviewOrder order = ReviewOrder.Date)
        {
            IOrderedQueryable<Review> ordered;
            switch (order)
            {
                case ReviewOrder.Date:
                    ordered=sourceQuery.OrderByDescending(x => x.CreatedDate);
                    break;
                case ReviewOrder.Likes:
                    ordered=sourceQuery.OrderByDescending(x => x.CachedLikes);

                    break;
                default:
                    ordered=sourceQuery.OrderByDescending(x => x.CreatedDate);
                    break;
            }
            return ordered.Where(x => x.Status==ReviewStatus.Published)
                .Skip(page*recordsPerPage)
                .Take(recordsPerPage)
                .Include(r => r.Rating)
                .ThenInclude(r => r.Author)
                .Include(r => r.Rating)
                .ThenInclude(r => r.MediaPiece);
        }
        private IQueryable<Guid> GetSearchQuery(ApplicationDbContext context, string searchQuery)
        {
            var foundByNameIds = context.Reviews.Where(x => x.Name.StartsWith(searchQuery)).Select(x => x.Id);
            var foundInReviewsIds = context.Reviews.Where(x => EF.Functions.FreeText(x.Content, searchQuery)).Select(x => x.Id);
            var foundInCommentsIds = context.Comments.Where(x => EF.Functions.FreeText(x.Content, searchQuery)).Select(x => x.ReviewId);
            var foundByTagIds = GetSearchByTagQuery(context, searchQuery);
            IQueryable<Guid> foundIds = foundInReviewsIds.Union(foundInCommentsIds).Union(foundByTagIds).Union(foundByNameIds).Distinct();
            return foundIds;
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
                return await context.Reviews.Include(x => x.Rating)
                    .ThenInclude(x => x.MediaPiece)
                    .Include(x => x.Rating)
                    .SingleOrDefaultAsync(x => x.Id == id);
            }
        }

        public override async Task<Guid> Save(Review entity, CancellationToken cancellationToken = default)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                if (entity.Id == default)
                {
                    context.Entry(entity).State = EntityState.Added;
                    entity.CreatedDate = DateTime.Now;
                }
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
                await Save(entity);
                return true;
            }
        }
    }
}
