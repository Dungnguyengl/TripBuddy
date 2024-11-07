using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
    public class DestinationDTO
    {
        public Guid DesKey { get; set; }
        public string? DesName { get; set; }
        public Guid? DesPic { get; set; }
        public string? DesDescription { get; set; }
        public Guid AtrKey { get; set; }
        public string? PicLink { get; set; }
    }

    public class SearcDestinationDTO : SearchQueryBase
    {

    }

    public class DestinationDetailDTO
    {
        public Guid DesKey { get; set; }
        public string? DesName { get; set; }
        public List<DetailPlaceDto>? Places { get; set; }
    }
}
