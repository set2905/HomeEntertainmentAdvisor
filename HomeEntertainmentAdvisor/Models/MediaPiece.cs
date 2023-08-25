namespace HomeEntertainmentAdvisor.Models
{
    public enum MediaGroup
    {
        Film,
        Game,
        Book
    }
    public class MediaPiece
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double CachedRating { get; set; }
        public MediaGroup Group { get; set; }
        public DateTime LastCacheUpdate { get; set; }
    }
}
