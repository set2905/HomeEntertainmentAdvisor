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
