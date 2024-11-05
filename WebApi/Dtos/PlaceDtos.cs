namespace WebApi.Dtos
{
    public class DetailPlaceQuery
    {
        public Guid Key { get; set; }
    }

    public class DetailPlaceDto
    {
        public Guid PlaceKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string PicLink { get; set; }
    }

    public class DropdownPlaceDTO
    {
        public DesKeyValueDTO Destination { get; set; }
        public List<PlaceKeyValueDTO> Places { get; set; }
    }

    public class DesKeyValueDTO
    {
        public Guid DesKey { get; set; }
        public string? DesName { get; set; }
    }

    public class PlaceKeyValueDTO
    {
        public Guid PlcKey { get; set; }
        public string? PlcName { get; set; }
    }
}
