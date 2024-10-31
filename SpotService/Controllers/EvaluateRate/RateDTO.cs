namespace SpotService.Controllers.EvaluateRate
{
    public class RateDTO
    {
        public Guid EvlKey { get; set; }
        public string EvlTitle { get; set; }
        public string EvlDescription { get; set; }
        public Guid PicKey { get; set; }
        public string AtrTitle { get; set; }
        public string DesTitle { get; set; }
        public string PlcTitle  { get; set; }
        public int Rate { get; set; }
    }

    public class CRateDTO
    {
        public string EvlTitle { get; set; }
        public string EvlDescription { get; set; }
        public Guid PicKey { get; set; }
        public int Rate { get; set; }
        public Guid PlcKey { get; set; }
    }
}
