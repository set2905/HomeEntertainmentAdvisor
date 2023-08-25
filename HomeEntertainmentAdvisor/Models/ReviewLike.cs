using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Models
{
    [PrimaryKey(nameof(ReviewId), nameof(UserId))]

    public class ReviewLike
    {
        public virtual Review Review { get; set; }
        public Guid ReviewId { get; set; }
        public virtual User User { get; set; }
        public Guid UserId { get; set; }
    }
}
