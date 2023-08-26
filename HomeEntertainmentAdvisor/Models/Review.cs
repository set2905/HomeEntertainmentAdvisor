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
        public virtual Rating Rating { get; set; }
        public Guid RatingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastCacheUpdate { get; set; }
    }
}
