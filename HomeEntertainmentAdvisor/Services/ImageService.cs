﻿using CloudinaryDotNet;
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
            cloudinary=new Cloudinary("");
        }

        public async Task<ImageUploadResult> UploadImage(IBrowserFile file, Guid reviewId)
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
                ReviewId=reviewId,
                CloudinaryPublicId=result.PublicId,
                FileName=file.Name
            };
            if (result.StatusCode==System.Net.HttpStatusCode.OK) await SaveReviewImage(reviewImage);
            return result;
        }
        public async Task<bool> RemoveImage(ReviewImage reviewImage)
        {
            DelResResult delResResult = await cloudinary.DeleteResourcesAsync(new string[] { reviewImage.CloudinaryPublicId.ToString() });
            if (delResResult.Deleted.First().Value!="deleted") return false;
            await imagesRepo.Delete(reviewImage);
            return true;
        }
        public async Task<Guid> SaveReviewImage(ReviewImage reviewImage)
        {
            return await imagesRepo.Save(reviewImage);
        }
        public async Task<List<ReviewImage>> GetImagesForReview(Guid reviewId)
        {
            return await imagesRepo.GetImagesForReview(reviewId);
        }
        public async Task<ReviewImage?> GetById(Guid id)
        {
            return await imagesRepo.GetById(id);
        }
    }
}