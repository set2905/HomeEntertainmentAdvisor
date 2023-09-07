using CloudinaryDotNet.Actions;
using HomeEntertainmentAdvisor.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IImageService
    {
        Task<ReviewImage?> GetById(Guid id);
        Task<List<ReviewImage>> GetImagesForReview(Guid reviewId);
        Task<bool> RemoveImage(ReviewImage reviewImage);
        Task<Guid> SaveReviewImage(ReviewImage reviewImage);
        public Task<ImageUploadResult> UploadImage(IBrowserFile file, Guid reviewId);

    }
}