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
        /// <summary>
        /// Erases resources with spicified public Ids from cloud
        /// </summary>
        /// <param name="publicIds"></param>
        /// <returns></returns>
        public Task<DelResResult> DeleteResourcesAsync(params string[] publicIds)
        {
            return cloudinary.DeleteResourcesAsync(publicIds);
        }
        /// <summary>
        /// Uploads image to the cloud
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result of the upload</returns>
        public Task<ImageUploadResult> UploadImageAsync(ImageUploadParams parameters, CancellationToken? cancellationToken = default)
        {
            return cloudinary.UploadAsync(parameters, cancellationToken);
        }
    }
}
