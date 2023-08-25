using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class ReviewTagRelationsRepo : RepoBase<ReviewTagRelation, (Guid, Guid)>, IReviewTagRelationsRepo
    {
        public ReviewTagRelationsRepo(ApplicationDbContext context) : base(context)
        {
            dbSet=context.ReviewTagRelations;
        }

        public override async Task<ReviewTagRelation?> GetById((Guid, Guid) id)
        {
            return await dbSet.SingleOrDefaultAsync(x => x.ReviewId == id.Item1&&x.TagId==id.Item2);

        }

        public override async Task<(Guid, Guid)> Save(ReviewTagRelation entity)
        {
            context.Entry(entity).State = EntityState.Added;
            await context.SaveChangesAsync();
            return (entity.ReviewId, entity.TagId);
        }
    }
}
