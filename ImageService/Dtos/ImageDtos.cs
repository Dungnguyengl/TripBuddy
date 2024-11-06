namespace ImageService.Dtos
{
    public class GetImageLinkQuery
    {
        public Guid Id { get; set; }
    }

    public class GetImageLinkDto
    {
        public string LinkImage { get; set; }
    }

    public class DeleteImageCommand
    {
        public Guid Id { get; set; }
    }

    public class DeleteImageDto
    {
        public string Message { get; set; }
    }

    public class RestoreImageCommand
    {
        public Guid Id { get; set; }
    }

    public class RestoreImageDto
    {
        public string Message { get; set; }
    }
}
