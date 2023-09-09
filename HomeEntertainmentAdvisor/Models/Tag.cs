using System.ComponentModel.DataAnnotations;

namespace HomeEntertainmentAdvisor.Models
{
    public class Tag
    {
        public Tag()
        {
            Name=string.Empty;
        }

        public Guid Id { get; set; }
        [StringLength(128, MinimumLength = 1)]
        public string Name { get; set; }
        public int Popularity { get; set; }
    }
}
