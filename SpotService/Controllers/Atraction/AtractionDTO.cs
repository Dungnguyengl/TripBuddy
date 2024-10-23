using SpotService.Model;

namespace SpotService.Controllers.Atraction
{
    public class AtractionDTO
    {
        public Guid Id { get; set; }
        public string Continent { get; set; }
        public string SubContinent { get; set; }
        public string Country { get; set; }
        public string PicLink { get; set; }
    }

    public class DAttractionDTO : AtrHead
    {

    }

    public class AttractionKeyValueDTO
    {
        public Guid AtrKey { get; set; }
        public string? Country { get; set; }
    }
}
