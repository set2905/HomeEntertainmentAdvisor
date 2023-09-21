using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HomeEntertainmentAdvisor.Services.Interfaces;

namespace HomeEntertainmentAdvisor.Services
{
    public class CloudStorage : ICloudStorage
    {
        private readonly Cloudinary cloudinary;

        public CloudStorage(string? cloudinaryUrl)
        {
            if (cloudinaryUrl == null) throw new ArgumentNullException(nameof(cloudinaryUrl));
            cloudinary=new Cloudinary(cloudinaryUrl);
        }

        public Task<DelResResult> DeleteResourcesAsync(params string[] publicIds)
        {
            return cloudinary.DeleteResourcesAsync(publicIds);
        }

        public Task<ImageUploadResult> UploadImageAsync(ImageUploadParams parameters, CancellationToken? cancellationToken = default)
        {
            return cloudinary.UploadAsync(parameters, cancellationToken);
        }
    }
}
