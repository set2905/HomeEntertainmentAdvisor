using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<string>> SearchByName(string query);
        Task<bool> AddTags(Guid reviewId, IEnumerable<string> tags);
        Task<bool> OverwriteTags(Guid reviewId, IEnumerable<string> tags);
        Task<List<Tag>> GetReviewTags(Guid reviewId, int take = 0);
        Task<List<Tag>> GetTags(int skip = 0, int take = 10, CancellationToken cancellationToken = default);
    }
}