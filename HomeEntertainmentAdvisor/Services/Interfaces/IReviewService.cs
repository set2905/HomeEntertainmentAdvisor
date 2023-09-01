﻿using HomeEntertainmentAdvisor.Models;
using MudBlazor;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<Review>> FindReviews(string query, int page = 0, int perPage = 10);
        Task<List<Review>> GetNewest();
        Task<Review?> GetById(Guid id);
        Task<(Guid id, bool succeeded, string message)> TrySaveReview(Review review);
    }
}
