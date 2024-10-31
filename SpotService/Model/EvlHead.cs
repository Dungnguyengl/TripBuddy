using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotService.Model
{
    [Table("EVL_HEAD", Schema = "EVALUATE")]
    [Index("EvlKey", Name = "EVL_PLC_index_1")]
    public class EvlHead
    {
        [Key]
        [Column("EVL_KEY")]
        public Guid EvlKey { get; set; }
        [Column("EVL_TITLE")]
        [StringLength(100)]
        public string EvlTitle { get; set; }
        [Column("EVL_CONTENT")]
        [StringLength(255)]
        public string EvlContent { get; set; }
        [Column("PIC_KEY")]
        public Guid PicKey { get; set; }
    }
}
