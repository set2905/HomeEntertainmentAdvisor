using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class ReviewLikesRepo : RepoBase<ReviewLike, (Guid, string)>, IReviewLikesRepo
    {
        public ReviewLikesRepo(ApplicationDbContext context) : base(context)
        {
            dbSet=context.ReviewLikes;
        }

        public override async Task<ReviewLike?> GetById((Guid, string) id)
        {
            return await dbSet.SingleOrDefaultAsync(x => x.ReviewId == id.Item1&&x.UserId==id.Item2);

        }

        public override async Task<(Guid, string)> Save(ReviewLike entity)
        {
            context.Entry(entity).State = EntityState.Added;
            await context.SaveChangesAsync();
            return (entity.ReviewId, entity.UserId);
        }
    }
}
