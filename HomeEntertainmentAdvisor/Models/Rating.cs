using System.ComponentModel.DataAnnotations;

namespace HomeEntertainmentAdvisor.Models
{
    public class Rating
    {
        public Guid Id { get; set; }
        public virtual User? Author { get; set; }
        public string? AuthorId { get; set; }
        public virtual MediaPiece MediaPiece { get; set; }
        public Guid MediaPieceId { get; set; }
        [Range(0, 10)]
        public int Grade { get; set; }
    }
}
