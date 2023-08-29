using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IRatingService
    {
        Task<Guid> SaveRating(Rating rating);
    }
}
