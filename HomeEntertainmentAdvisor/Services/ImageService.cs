using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HomeEntertainmentAdvisor.Domain.Repo.Interfaces;
using HomeEntertainmentAdvisor.Models;
using HomeEntertainmentAdvisor.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;

namespace HomeEntertainmentAdvisor.Services
{
    public class ImageService : IImageService
    {
        private readonly IReviewImagesRepo imagesRepo;
        private readonly Cloudinary cloudinary;
        public ImageService(IReviewImagesRepo imagesRepo)
        {
            this.imagesRepo=imagesRepo;
            //Tecт!!!
            cloudinary=new Cloudinary("url");
        }

        public async Task<Guid> UploadImage(IBrowserFile file, Guid reviewId)
        {
            ImageUploadResult result = new();
            using (var stream = file.OpenReadStream())
            {
                ImageUploadParams uploadParams = new()
                {
                    File=new FileDescription(file.Name, stream)
                };
                result=await cloudinary.UploadAsync(uploadParams);
            }
            ReviewImage reviewImage = new()
            {
                Url=result.Url.ToString(),
                ReviewId=reviewId
            };
            return await SaveReviewImage(reviewImage);
        }
        public async Task<Guid> SaveReviewImage(ReviewImage reviewImage)
        {
            return await imagesRepo.Save(reviewImage);
        }
        public async Task<List<ReviewImage>> GetImagesForReview(Guid reviewId)
        {
            throw new NotImplementedException();
        }
        public async Task<ReviewImage> GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
