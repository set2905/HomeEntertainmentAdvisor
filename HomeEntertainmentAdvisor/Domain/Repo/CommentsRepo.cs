using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class CommentsRepo : RepoBase<Comment, Guid>, ICommentsRepo
    {
        public CommentsRepo(ApplicationDbContext context) : base(context)
        {
            dbSet=context.Comments;
        }

        public override async Task<Comment?> GetById(Guid id)
        {
            return await dbSet.SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<Guid> Save(Comment entity)
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
