using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotService.Model
{
    [Table("EVL_RATE", Schema = "EVALUATE")]
    [Index("EvlKey", Name = "EVL_PLC_index_1")]
    public class EvlRate
    {
        [Key]
        [Column("EVL_KEY")]
        public Guid EvlKey { get; set; }
        [Column("RATE")]
        public int Rate { get; set; }
    }
}
