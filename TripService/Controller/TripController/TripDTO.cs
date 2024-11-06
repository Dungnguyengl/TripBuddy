using System.ComponentModel.DataAnnotations.Schema;

namespace TripperService.Controller.TripController
{
    public class TripDTO
    {
        public Guid TripKey { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PlcType { get; set; } = null!;
        public string? PlcContent { get; set; }
        public Guid? ReferentKey { get; set; }
        public Guid PicKey { get; set; }
        public bool IsVisited { get; set; }
    }

    public class CTripDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PlcType { get; set; } = null!;
        public string? PlcContent { get; set; }
        public Guid? ReferentKey { get; set; }
    }

    public class UTripDTO
    {
        public Guid TripKey { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PlcType { get; set; } = null!;
        public string? PlcContent { get; set; }
        public Guid? ReferentKey { get; set; }
    }
}
