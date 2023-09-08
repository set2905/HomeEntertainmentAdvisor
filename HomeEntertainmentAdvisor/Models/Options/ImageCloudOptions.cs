namespace HomeEntertainmentAdvisor.Models.Options
{
    public class ImageCloudOptions
    {
        public ImageCloudOptions(string url)
        {
            Url=url;
        }

        public string Url { get; set; } = string.Empty;
    }
}
