using Microsoft.EntityFrameworkCore;

namespace HomeEntertainmentAdvisor.Models
{
    [PrimaryKey(nameof(TagId), nameof(ReviewId))]

    public class ReviewTagRelation
    {
            public virtual Tag Tag { get; set; }
            public virtual Review Review { get; set; }
            public Guid TagId { get; set; }
            public Guid ReviewId { get; set; }
        
    }
}
