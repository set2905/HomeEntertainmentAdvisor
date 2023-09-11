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
    class CommentComparer : IEqualityComparer<Comment>
    {
        public bool Equals(Comment? x, Comment? y)
        {
            if (x == null || y == null)
                return false;

            if (ReferenceEquals(x, y))
                return true;
            return x.Id == y.Id;
        }

        public int GetHashCode(Comment? obj)
        {
            if (obj == null || obj.Id == default)
                return 0;
            return obj.Id.GetHashCode();
        }
    }
}
