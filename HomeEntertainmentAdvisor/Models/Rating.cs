using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HomeEntertainmentAdvisor.Models
{
    [PrimaryKey(nameof(AuthorId), nameof(MediaPieceId))]

    public class Rating
    {
        public const int MAX_RATING = 10;

        public Rating()
        {
            MediaPiece=new();
        }

        public virtual User? Author { get; set; }
        public string? AuthorId { get; set; }
        public virtual MediaPiece MediaPiece { get; set; }
        public Guid MediaPieceId { get; set; }
        [Range(0, MAX_RATING)]
        public int Grade { get; set; }
    }

    public class RatingValidator : AbstractValidator<Rating>
    {
        public RatingValidator()
        {
            RuleFor(r => r.Grade)
               .NotNull().WithMessage("You must enter the rating of the review")
               .GreaterThanOrEqualTo(1).WithMessage($"Rating should not be less than 1")
               .LessThanOrEqualTo(Rating.MAX_RATING).WithMessage($"Rating should not be greater than {Rating.MAX_RATING}");
            RuleFor(r => r.MediaPiece.Id)
                .NotEqual(Guid.Empty).WithMessage("Select media piece to review");

        }

    }
}
