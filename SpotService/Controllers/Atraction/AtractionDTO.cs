using SpotService.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

    public class DAttractionDTO
    {
        public Guid AtrKey { get; set; }
        public string? Description { get; set; }
        public string? Contitnent { get; set; }
        public string? SubContitnent { get; set; }
        public string? Country { get; set; }
        public string? PicLink { get; set; }
        public IEnumerable<AtrContent> AtrContents { get; set; } = new List<AtrContent>();
        public IEnumerable<DesHead> DesHeads { get; set; } = new List<DesHead>();
        public IEnumerable<PlcHead> PlcHeads { get; set; } = new List<PlcHead>();
    }

    public class AttractionKeyValueDTO
    {
        public Guid AtrKey { get; set; }
        public string? Country { get; set; }
    }
}
