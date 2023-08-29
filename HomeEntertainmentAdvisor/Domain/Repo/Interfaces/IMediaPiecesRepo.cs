using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Domain.Repo.Interfaces
{
    public interface IMediaPiecesRepo : IRepo<MediaPiece, Guid>
    {
        Task<List<MediaPiece>> Search(string value, CancellationToken cancellationToken);
    }
}
