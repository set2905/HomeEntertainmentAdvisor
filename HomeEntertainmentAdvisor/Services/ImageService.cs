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
        private readonly ICloudStorage imageCloud;

        public ImageService(IReviewImagesRepo imagesRepo, ICloudStorage imageCloud)
        {
            this.imagesRepo=imagesRepo;
            this.imageCloud=imageCloud;
        }
        /// <summary>
        /// Uploads image to the cloud from browser file and saves record to the db
        /// </summary>
        /// <param name="file"></param>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public async Task<ImageUploadResult> UploadImage(IBrowserFile file, Guid reviewId)
        {
            ImageUploadResult result = new();
            using (Stream stream = file.OpenReadStream())
            {
                ImageUploadParams uploadParams = new()
                {
                    File=new FileDescription(file.Name, stream)
                };
                result=await imageCloud.UploadImageAsync(uploadParams);
            }
            ReviewImage reviewImage = new()
            {
                Url=result.Url.ToString(),
                ReviewId=reviewId,
                CloudinaryPublicId=result.PublicId,
                FileName=file.Name
            };
            if (result.StatusCode==System.Net.HttpStatusCode.OK) await SaveReviewImage(reviewImage);
            return result;
        }
        /// <summary>
        /// Deletes review image from the cloud
        /// </summary>
        /// <param name="reviewImage"></param>
        /// <returns></returns>
        public async Task<bool> RemoveImage(ReviewImage reviewImage)
        {
            DelResResult delResResult = await imageCloud.DeleteResourcesAsync(new string[] { reviewImage.CloudinaryPublicId.ToString() });
            if (delResResult.Deleted.First().Value!="deleted") return false;
            await imagesRepo.Delete(reviewImage);
            return true;
        }
        /// <summary>
        /// Saves review image record to db
        /// </summary>
        /// <param name="reviewImage"></param>
        /// <returns></returns>
        public async Task<Guid> SaveReviewImage(ReviewImage reviewImage)
        {
            return await imagesRepo.Save(reviewImage);
        }
        /// <summary>
        /// Gets all images for specific review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public async Task<List<ReviewImage>> GetImagesForReview(Guid reviewId)
        {
            return await imagesRepo.GetImagesForReview(reviewId);
        }
        /// <summary>
        /// Gets first image in review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns>First image in review or null</returns>
        public async Task<string?> GetFirstImageUrl(Guid reviewId)
        {
            return await imagesRepo.GetFirstImageUrl(reviewId);
        }
        /// <summary>
        /// Gets review image by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ReviewImage?> GetById(Guid id)
        {
            return await imagesRepo.GetById(id);
        }
    }
}
