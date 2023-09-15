using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IMediaService
    {
        Task<List<MediaPiece>> SearchMediaPieces(string value, CancellationToken cancellationToken);
        Task<List<MediaPiece>> GetAll();
        Task Save(MediaPiece media);
        Task<List<MediaGroup>> SearchMediaGroups(string value, CancellationToken cancellationToken);
    }
}
