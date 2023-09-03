using HomeEntertainmentAdvisor.Data;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace HomeEntertainmentAdvisor.Domain.Repo
{
    public class TagRepo : RepoBase<Tag, Guid>, ITagRepo
    {
        public TagRepo(ApplicationDbContext context) : base(context)
        {
            dbSet=context.Tags;
        }

        public async Task<Tag?> GetByName(string name)
        {
            return await context.Tags.SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<Tag>> SearchByName(string query)
        {
            return await dbSet.Where(x => x.Name.StartsWith(query)).ToListAsync();
        }
        public override async Task<Tag?> GetById(Guid id)
        {
            return await dbSet.SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<Guid> Save(Tag entity)
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
