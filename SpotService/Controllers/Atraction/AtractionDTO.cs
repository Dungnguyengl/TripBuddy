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

        public Guid? PicKey { get; set; }
        public virtual ICollection<AtrContent> AtrContents { get; set; } = new List<AtrContent>();

        public virtual ICollection<DesHead> DesHeads { get; set; } = new List<DesHead>();

        public virtual ICollection<PlcHead> PlcHeads { get; set; } = new List<PlcHead>();
    }

    public class AttractionKeyValueDTO
    {
        public Guid AtrKey { get; set; }
        public string? Country { get; set; }
    }
}
