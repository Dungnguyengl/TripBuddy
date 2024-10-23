namespace SpotService.Controllers.Place
{
    public class PlaceDTO : CPlaceDTO
    {
        public Guid PlaceKey { get; set; }

    }

    public class CPlaceDTO
    {
        public Guid AtrKey { get; set; }
        public Guid DesKey { get; set; }
        public string? PlaceName { get; set; }
        public string? Description { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public Guid? PicKey { get; set; }
    }

    public class PlaceKeyValueDTO
    {
        public Guid PlcKey { get; set; }
        public string? PlcName { get; set; }
    }
}
