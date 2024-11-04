using System.ComponentModel.DataAnnotations;

namespace SpotService.Controllers.Story
{
    public class StoryDTO
    {
        [Key]
        public Guid StoryKey { get; set; }
        public Guid AtrKey { get; set; }
        public string? Country { get; set; }
        public string? Destination { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
