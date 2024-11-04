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
}
