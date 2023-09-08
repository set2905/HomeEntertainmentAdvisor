using CloudinaryDotNet.Actions;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IImageCloud
    {
        public Task<DelResResult> DeleteResourcesAsync(params string[] publicIds);
        public Task<ImageUploadResult> UploadAsync(ImageUploadParams parameters, CancellationToken? cancellationToken = null);
    }
}
