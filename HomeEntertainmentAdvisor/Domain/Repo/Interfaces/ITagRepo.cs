using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface ITagRepo : IRepo<Tag, Guid>
    {
        Task<Tag?> GetByName(string name);
        Task<List<Tag>> GetTags(int skip = 0, int take = 10, CancellationToken cancellationToken = default);
        Task<List<Tag>> SearchByName(string query, CancellationToken cancellationToken = default);
    }
}
