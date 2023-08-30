using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;

namespace HomeEntertainmentAdvisor.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaPiecesRepo mediaPiecesRepo;

        public MediaService(IMediaPiecesRepo mediaPiecesRepo)
        {
            this.mediaPiecesRepo=mediaPiecesRepo;
        }
        public async Task<List<MediaPiece>> GetAll()
        {
            return await mediaPiecesRepo.GetAll();
        }
        public async Task<List<MediaPiece>> Search(string value, CancellationToken cancellationToken)
        {
            return await mediaPiecesRepo.Search(value, cancellationToken);
        }
        public async Task Save(MediaPiece media)
        {
            await mediaPiecesRepo.Save(media);
        }
    }
}
