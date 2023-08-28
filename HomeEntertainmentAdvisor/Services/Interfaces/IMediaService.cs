using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IMediaService
    {
        Task<List<MediaPiece>> Search(string value);
        Task<List<MediaPiece>> GetAll();
    }
}
