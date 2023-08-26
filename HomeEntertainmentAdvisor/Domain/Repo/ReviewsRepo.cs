using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class ReviewsRepo : RepoBase<Review, Guid>, IReviewsRepo
    {
        public ReviewsRepo(ApplicationDbContext context) : base(context)
        {
            dbSet=context.Reviews;
        }

        public async Task<List<Review>> GetPage(int page, int recordsPerPage)
        {
            if (page<1) throw new ArgumentException("page cannot be less than 1");
            if (recordsPerPage<1) throw new ArgumentException("recordsPerPage cannot be less than 1");
            return await dbSet.OrderBy(x => x.CreatedDate).Skip((page-1)*recordsPerPage).Take(recordsPerPage).ToListAsync();
        }

        public async Task<List<Review>> GetPage(int page, int recordsPerPage, string searhQuery)
        {
            IQueryable<Guid> foundIds = dbSet
                .Join(context.Comments, r => r.Id, c => c.ReviewId, (r, c) => new { reviewId = r.Id, reviewContent = r.Content, reviewName = r.Name, comment = c.Content })
                .Where(x => EF.Functions.FreeText(x.reviewContent, searhQuery)/*||EF.Functions.FreeText(x.reviewName, searhQuery)*/||EF.Functions.FreeText(x.comment, searhQuery))
                .Select(x => x.reviewId);
            IQueryable<Review> found = dbSet.Where(x => foundIds.Contains(x.Id));
            return await found.OrderBy(x => x.CreatedDate).Skip(page*recordsPerPage).Take(recordsPerPage).ToListAsync();
        }
        public override async Task<Review?> GetById(Guid id)
        {
            return await dbSet.SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<Guid> Save(Review entity)
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
