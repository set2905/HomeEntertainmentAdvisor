using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HomeEntertainmentAdvisor.Services.Interfaces;

namespace HomeEntertainmentAdvisor.Services
{
    public class ImageCloud : IImageCloud
    {
        private readonly Cloudinary cloudinary;

        public ImageCloud(string? cloudinaryUrl)
        {
            if (cloudinaryUrl == null) throw new ArgumentNullException(nameof(cloudinaryUrl));
            cloudinary=new Cloudinary(cloudinaryUrl);
        }

        public Task<DelResResult> DeleteResourcesAsync(params string[] publicIds)
        {
            return cloudinary.DeleteResourcesAsync(publicIds);
        }

        public Task<ImageUploadResult> UploadAsync(ImageUploadParams parameters, CancellationToken? cancellationToken = null)
        {
            return cloudinary.UploadAsync(parameters);
        }
    }
}
