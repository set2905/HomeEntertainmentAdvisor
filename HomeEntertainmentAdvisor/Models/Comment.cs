using System.ComponentModel.DataAnnotations;

namespace HomeEntertainmentAdvisor.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public virtual User Author { get; set; }
        public Guid AuthorId { get; set; }
        public virtual Review Review { get; set; }
        public Guid ReviewId { get; set; }

        [StringLength(255, MinimumLength = 1)]
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
