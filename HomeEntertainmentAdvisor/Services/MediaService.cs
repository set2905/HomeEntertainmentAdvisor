using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;

namespace HomeEntertainmentAdvisor.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaPiecesRepo mediaPiecesRepo;
        private readonly IMediaPieceGroupsRepo groupsRepo;
        public MediaService(IMediaPiecesRepo mediaPiecesRepo, IMediaPieceGroupsRepo groupsRepo)
        {
            this.mediaPiecesRepo=mediaPiecesRepo;
            this.groupsRepo=groupsRepo;
        }
        public async Task<List<MediaPiece>> GetAll()
        {
            return await mediaPiecesRepo.GetAll();
        }
        public async Task<List<MediaGroup>> SearchMediaGroups(string value, CancellationToken cancellationToken)
        {
            return await groupsRepo.Search(value, cancellationToken);
        }
        public async Task<List<MediaPiece>> SearchMediaPieces(string value, CancellationToken cancellationToken)
        {
            return await mediaPiecesRepo.Search(value, cancellationToken);
        }
        public async Task Save(MediaPiece media)
        {
            await mediaPiecesRepo.Save(media);
        }
    }
}
