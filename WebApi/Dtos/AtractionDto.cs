namespace WebApi.Dtos
{
    public class SearchAtractionQuery
    {
        public string? Word { get; set; }
        public string? Continent { get; set; }
    }

    public class SearchAtractionDto
    {
        public Guid Id { get; set; }
        public string Continent { get; set; }
        public string SubContinent { get; set; }
        public string Country { get; set; }
        public string PicLink { get; set; }
    }
}
