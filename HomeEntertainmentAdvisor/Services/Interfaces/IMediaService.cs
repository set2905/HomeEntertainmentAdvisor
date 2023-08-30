﻿using HomeEntertainmentAdvisor.Models;

namespace HomeEntertainmentAdvisor.Services.Interfaces
{
    public interface IMediaService
    {
        Task<List<MediaPiece>> Search(string value, CancellationToken cancellationToken);
        Task<List<MediaPiece>> GetAll();
        Task Save(MediaPiece media);
    }
}
