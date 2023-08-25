using System.ComponentModel.DataAnnotations;

namespace HomeEntertainmentAdvisor.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        [StringLength(64, MinimumLength = 1)]
        public string Name { get; set; }
        [StringLength(4096, MinimumLength = 1)]
        public string Content { get; set; }
        public int CachedLikes { get; set; }
        public int Rating { get; set; }
        public virtual User? Author { get; set; }
        public string? AuthorId { get; set; }
        public virtual MediaPiece MediaPiece { get; set; }
        public Guid MediaPieceId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastCacheUpdate { get; set; }

    }
}
