using CloudinaryDotNet.Actions;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface ICloudStorage
    {
        public Task<DelResResult> DeleteResourcesAsync(params string[] publicIds);
        public Task<ImageUploadResult> UploadImageAsync(ImageUploadParams parameters, CancellationToken? cancellationToken = null);
    }
}
