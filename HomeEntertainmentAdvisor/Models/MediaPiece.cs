using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace HomeEntertainmentAdvisor.Models
{

    public class MediaPiece
    {
        public const int MAX_NAME = 128;
        public Guid Id { get; set; }
        [StringLength(MAX_NAME, MinimumLength = 1)]
        public string Name { get; set; }
        public double CachedRating { get; set; }
        public virtual MediaGroup? MediaGroup { get; set; }
        public int? MediaGroupId { get; set; }
        public DateTime LastCacheUpdate { get; set; }
    }
    public class MediaPieceValidator : AbstractValidator<MediaPiece>
    {
        public MediaPieceValidator()
        {
            RuleFor(r => r.Name)
               .NotEmpty().WithMessage("You must enter the name of the media")
               .MaximumLength(Review.MAX_NAME).WithMessage($"Name should not be longer than {Review.MAX_NAME}");
            RuleFor(r => r.MediaGroup)
                .NotNull().WithMessage("You must enter the group of the media");
        }

    }
}
