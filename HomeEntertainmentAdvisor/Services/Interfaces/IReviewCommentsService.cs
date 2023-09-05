using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IReviewCommentsService
    {
        Task<Guid> AddComment(string content, Guid reviewId);
        Task<List<Comment>> GetCommentPage(Guid reviewId, int page = 1, int pageSize = 10);
    }
}