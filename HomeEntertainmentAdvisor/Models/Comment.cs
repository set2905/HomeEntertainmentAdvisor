using System.ComponentModel.DataAnnotations;

namespace HomeEntertainmentAdvisor.Models
{

    public class Comment
    {
        public const int MAX_LENGTH = 255;

        public Guid Id { get; set; }
        public virtual User Author { get; set; }
        public string AuthorId { get; set; }
        public virtual Review Review { get; set; }
        public Guid ReviewId { get; set; }

        [StringLength(MAX_LENGTH, MinimumLength = 1)]
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
