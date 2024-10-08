using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotService.Model;

[Table("DES_HEAD", Schema = "SPOT")]
[Index("AtrKey", Name = "DES_HEAD_index_2")]
public partial class DesHead
{
    [Key]
    [Column("DES_KEY")]
    public Guid DesKey { get; set; }

    [Column("ATR_KEY")]
    public Guid AtrKey { get; set; }

    [Column("DES_NAME")]
    [StringLength(50)]
    public string? DesName { get; set; }

    [Column("DESCRIPTION")]
    public string? Description { get; set; }

    [Column("PIC_KEY")]
    public Guid? PicKey { get; set; }

    [Column("NO_VISTED")]
    public int? NoVisted { get; set; }

    [InverseProperty("DesKeyNavigation")]
    public virtual ICollection<AtrContent> AtrContents { get; set; } = new List<AtrContent>();

    [ForeignKey("AtrKey")]
    [InverseProperty("DesHeads")]
    public virtual AtrHead AtrKeyNavigation { get; set; } = null!;

    [InverseProperty("DesKeyNavigation")]
    public virtual ICollection<PlcHead> PlcHeads { get; set; } = new List<PlcHead>();
}
