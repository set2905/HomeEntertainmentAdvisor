using HomeEntertainmentAdvisor.Domain.Repo;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;

namespace HomeEntertainmentAdvisor.Services
{
    public class RatingService : IRatingService
    {
        private const int AVG_MEDIARATING_UPDATE_SECONDS = 3600;
        private readonly IRatingRepo ratingRepo;
        private readonly IMediaPiecesRepo mediaRepo;

        public RatingService(IRatingRepo ratingRepo, IMediaPiecesRepo mediaRepo)
        {
            this.ratingRepo=ratingRepo;
            this.mediaRepo=mediaRepo;
        }
        /// <summary>
        /// Updates the media piece rating
        /// </summary>
        /// <param name="media"></param>
        /// <returns>Cached rating or updated rating, depending on last cache update</returns>
        public async Task<double> TryUpdateMediaRating(MediaPiece media)
        {
            if (media.LastCacheUpdate < DateTime.Now-TimeSpan.FromSeconds(AVG_MEDIARATING_UPDATE_SECONDS))
            {
                double avgRating = await ratingRepo.GetAvgMediaRating(media.Id);
                media.LastCacheUpdate = DateTime.Now;
                media.CachedRating = avgRating;
                await mediaRepo.Save(media);
                return avgRating;
            }
            else
                return media.CachedRating;
        }
        /// <summary>
        /// Gets rating record by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Rating?> GetById((string,Guid) id)
        {
            return await ratingRepo.GetById(id);
        }
        /// <summary>
        /// Gets rating by media and user id
        /// </summary>
        /// <param name="media"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Rating?> GetByMedia(MediaPiece media, string userId)
        {
            return await ratingRepo.GetRating(userId, media.Id);
        }
        /// <summary>
        /// Saves rating to db
        /// </summary>
        /// <param name="rating"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<(string, Guid)> SaveRating(Rating rating, string userId)
        {
            rating.AuthorId=userId;
            (string, Guid) id = await ratingRepo.Save(rating);
            return id;
        }
    }
}
