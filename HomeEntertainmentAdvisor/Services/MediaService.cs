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
        /// <summary>
        /// Gets all existing media pieces
        /// </summary>
        /// <returns></returns>
        public async Task<List<MediaPiece>> GetAll()
        {
            return await mediaPiecesRepo.GetAll();
        }
        /// <summary>
        /// Searches for groups of media
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<MediaGroup>> SearchMediaGroups(string value, CancellationToken cancellationToken)
        {
            return await groupsRepo.Search(value, cancellationToken);
        }
        /// <summary>
        /// Searches for media pieces
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<MediaPiece>> SearchMediaPieces(string value, CancellationToken cancellationToken)
        {
            return await mediaPiecesRepo.Search(value, cancellationToken);
        }
        /// <summary>
        /// Saves media piece to db
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        public async Task Save(MediaPiece media)
        {
            await mediaPiecesRepo.Save(media);
        }
    }
}
