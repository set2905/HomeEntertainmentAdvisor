namespace HomeEntertainmentAdvisor.Models
{
    public class ReviewImage
    {
        public const int MAXIMAGES_PERREVIEW = 5;
        public ReviewImage()
        {
            CloudinaryPublicId=string.Empty;
            Url=string.Empty;
        }

        public Guid Id { get; set; }
        public string CloudinaryPublicId { get; set; }
        public string FileName { get; set; }    
        public string Url { get; set; }
        public virtual Review Review { get; set; }
        public Guid ReviewId { get; set; }
    }
}
