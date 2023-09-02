using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using static MudBlazor.CategoryTypes;

namespace HomeEntertainmentAdvisor.Models
{
    public enum ReviewStatus
    {
        Published,
        Draft,
        Deleted    
    }
    public class Review
    {
        public const int MAX_NAME = 64;
        public const int MAX_CONTENT = 4096;
        public Review()
        {
            Rating=new();
            RatingId=Rating.Id;
            Name=string.Empty;
            Content=string.Empty;
            Id=Guid.Empty;
        }

        public Guid Id { get; set; }
        [StringLength(MAX_NAME, MinimumLength = 1)]
        public string Name { get; set; }
        [StringLength(MAX_CONTENT, MinimumLength = 1)]
        public string Content { get; set; }
        public int CachedLikes { get; set; }
        public virtual Rating Rating { get; set; }
        public Guid RatingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastCacheUpdate { get; set; }
        public ReviewStatus Status { get; set; }
    }
    public class ReviewValidator : AbstractValidator<Review>
    {
        public ReviewValidator()
        {
            RuleFor(r => r.Name)
               .NotNull().WithMessage("You must enter the name of the review")
               .NotEmpty().WithMessage("You must enter the name of the review")
               .MaximumLength(Review.MAX_NAME).WithMessage($"Name should not be longer than {Review.MAX_NAME} chars");
            RuleFor(r => r.Content)
               .NotNull().WithMessage("You must enter the content of the review")
               .NotEmpty().WithMessage("You must enter the content of the review")
               .MaximumLength(Review.MAX_CONTENT).WithMessage($"Content should not be longer than {Review.MAX_CONTENT} chars");
            RuleFor(r => r.Rating).SetValidator(new RatingValidator());
        }

    }
}
