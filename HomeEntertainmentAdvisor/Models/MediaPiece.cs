namespace HomeEntertainmentAdvisor.Models
{

    public class MediaPiece
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double CachedRating { get; set; }
        public virtual MediaGroup? MediaGroup { get; set; }
        public int? MediaGroupId { get; set; }
        public DateTime LastCacheUpdate { get; set; }
    }
}
