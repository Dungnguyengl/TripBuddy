namespace SpotService.Controllers.Destination
{
    public class DestinationDTO
    {
        public Guid DesKey { get; set; }
        public string DesName { get; set; }
        public Guid? DesPic { get; set; }
        public string DesDescription { get; set; }
        public Guid AtrKey { get; set; }
        
    }

    public class CDestinationDTO
    {
        public string DesName { get; set; }
        public string DesPic { get; set; }
        public string DesDescription { get; set; }
        public Guid AtrKey { get; set; }
    }
    public class UDestinationDto : CDestinationDTO
    {
        public Guid DesKey { get; set; }
    }
    public class DesKeyValueDTO
    {
        public Guid DesKey { get; set; }
        public string? DesName { get; set; }
    }
}
