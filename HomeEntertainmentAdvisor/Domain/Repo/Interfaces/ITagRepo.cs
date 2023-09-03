using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface ITagRepo : IRepo<Tag, Guid>
    {
        Task<Tag?> GetByName(string name);
        Task<List<Tag>> SearchByName(string query);
    }
}
