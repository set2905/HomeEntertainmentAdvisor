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
        public async Task<Rating?> GetById(Guid id)
        {
            return await ratingRepo.GetById(id);
        }
        public async Task<Rating?> GetByMedia(MediaPiece media, string userId)
        {
            return await ratingRepo.GetByMediaAndUser(userId, media.Id);
        }
        public async Task<Guid> SaveRating(Rating rating, string userId)
        {
            rating.AuthorId=userId;
            Guid id = await ratingRepo.Save(rating);
            return id;
        }
    }
}
