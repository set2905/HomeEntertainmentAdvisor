namespace HomeEntertainmentAdvisor.Models
{
    public class ReviewImage
    {
        public Guid Id { get; set; }    
        public string Url { get; set; }
        public virtual Review Review { get; set; }
        public Guid ReviewId { get; set; }
    }
}
