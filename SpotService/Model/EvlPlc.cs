using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotService.Model
{
    [Table("EVL_PLC", Schema = "EVALUATE")]
    [Index("AtrKey","PlcKey", "DesKey", Name = "EVL_PLC_index_2")]
    public class EvlPlc
    {
        [Key]
        [Column("EVL_KEY")]
        public Guid EvlKey { get; set; }
        [Column("ATR_KEY")]
        public Guid AtrKey { get; set; }
        [Column("DES_KEY")]
        public Guid DesKey { get; set; }
        [Column("PLC_KEY")]
        public Guid PlcKey { get; set; }
        [Column("PLC_CONENT")]
        [StringLength(100)]
        public string PlcContent {  get; set; }

        [ForeignKey("AtrKey")]
        public virtual AtrHead AtrKeyNavigation { get; set; } = null!;
        [ForeignKey("DesKey")]
        public virtual DesHead DesKeyNavigation { get; set; } = null!;
        [ForeignKey("PlcKey")]
        public virtual PlcHead PlcKeyNavigation { get; set; } = null!;
    }
}
