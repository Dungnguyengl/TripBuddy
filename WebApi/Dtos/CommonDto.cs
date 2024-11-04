namespace WebApi.Dtos
{
    public class SearchQueryBase
    {
        public int Page { get; set; } = 1;
        public int Items { get; set; } = 10;
        public int Skips { get => (Page - 1) * Items; }
    }
}
