using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IReviewExporter
    {
        Task ExportReview(Review review);
    }
}