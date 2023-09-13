using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;

namespace HomeEntertainmentAdvisor.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepo ratingRepo;

        public RatingService(IRatingRepo ratingRepo)
        {
            this.ratingRepo=ratingRepo;
        }


        public async Task<Rating?> GetById(Guid id)
        {
            return await ratingRepo.GetById(id);
        }
        public async Task<Rating?> GetByMedia(MediaPiece media, string userId)
        {
            return await ratingRepo.GetByMedia(userId, media.Id);
        }
        public async Task<Guid> SaveRating(Rating rating, string userId)
        {
            rating.AuthorId=userId;
            Guid id = await ratingRepo.Save(rating);
            return id;
        }
    }
}
