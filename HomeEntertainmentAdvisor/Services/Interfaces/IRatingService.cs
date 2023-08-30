using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IRatingService
    {
        Task<Rating?> GetById(Guid id);
        Task<Guid> SaveRating(Rating rating);
    }
}
