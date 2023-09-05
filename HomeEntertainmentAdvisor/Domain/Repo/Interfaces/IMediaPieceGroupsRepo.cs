using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IMediaPieceGroupsRepo : IRepo<MediaGroup, int>
    {
        Task<List<MediaGroup>> Search(string value, CancellationToken cancellationToken = default);
    }
}
