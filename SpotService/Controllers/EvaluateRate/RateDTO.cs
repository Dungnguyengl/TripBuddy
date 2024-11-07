namespace SpotService.Controllers.EvaluateRate
{
    public class RateDTO
    {
        public Guid PlcKey { get; set; }
        public Guid PicKey { get; set; }
        public string? PlcName { get; set; }
        public string? DesName { get; set; }
        public string? AtrName { get; set; }
        public string? PlcDescription { get; set; }
        public float AverageRate { get; set; }
    }

    public class CRateDTO
    {
        public string EvlTitle { get; set; }
        public string EvlDescription { get; set; }
        public int Rate { get; set; }
        public Guid PlcKey { get; set; }
        public Guid PicKey { get; set; }
    }
}
