namespace WebApi.Dtos
{
    public class SearchAtractionQuery : SearchQueryBase
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

    public class AtractionDetailQuery
    {
        public Guid AtrId { get; set; }
    }

    public class AtractionDetailDto
    {
        public Guid AtrKey { get; set; }
        public string? Description { get; set; }
        public string? Contitnent { get; set; }
        public string? SubContitnent { get; set; }
        public string? Country { get; set; }
        public IEnumerable<ChildDestinationDto> Destinations { get; set; }
        public IEnumerable<ChildPlaceDto> Places { get; set; }
        public IEnumerable<ChildStoryDto> Stories { get; set; }
    }

    public class ChildDestinationDto
    {
        public string? Name { get; set; }
        public string? PicLink { get; set; }
    }

    public class ChildPlaceDto
    {
        public string? Name { get; set; }
        public string? PicLink { get; set; }
    }
    public class ChildStoryDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Country { get; set; }
        public string? Destination { get; set; }
    }
}
