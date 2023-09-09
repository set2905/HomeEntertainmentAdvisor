﻿using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<Review>> GetPage(int page, int recordsPerPage, ReviewOrder order = ReviewOrder.Date)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Review>();
                return await GetPageQuery(dbSet, page, recordsPerPage, context, order).ToListAsync();
            }
        }
        public async Task<List<Review>> GetPage(int page, int recordsPerPage, IEnumerable<Tag> tags)
        {
            using (ApplicationDbContext context = contextFactory.CreateDbContext())
            {
                IQueryable<Review> sourceQuery = context.ReviewTagRelations
                    .Include(x => x.Tag)
                    .Where(x => tags.Contains(x.Tag))
                    .Select(x => x.Review)
                    .Distinct();

                return await GetPageQuery(sourceQuery, page, recordsPerPage, context).ToListAsync();
            }
        }

        public async Task<List<Review>> GetPage(int page, int recordsPerPage, string searchQuery)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var dbSet = context.Set<Review>();
                IQueryable<Guid> foundIds = GetSearchQuery(context, searchQuery);
                IQueryable<Review> found = dbSet.Where(x => foundIds.Contains(x.Id));
                return await GetPageQuery(found, page, recordsPerPage, context).ToListAsync();
            }
        }
        private IQueryable<Review> GetPageQuery(IQueryable<Review> sourceQuery, int page, int recordsPerPage, ApplicationDbContext context, ReviewOrder order = ReviewOrder.Date)
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
                .ThenInclude(r => r.Author);
        }
        private IQueryable<Guid> GetSearchQuery(ApplicationDbContext context, string searchQuery)
        {
            var foundInReviewsIds = context.Reviews.Where(x => EF.Functions.FreeText(x.Content, searchQuery)).Select(x => x.Id);
            var foundInCommentsIds = context.Comments.Where(x => EF.Functions.FreeText(x.Content, searchQuery)).Select(x => x.ReviewId);
            var foundByTagIds = GetSearchByTagQuery(context, searchQuery);
            IQueryable<Guid> foundIds = foundInReviewsIds.Union(foundInCommentsIds).Union(foundByTagIds).Distinct();
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
