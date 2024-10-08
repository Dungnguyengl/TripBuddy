using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotService.Model;

[Table("ATR_HEAD", Schema = "SPOT")]
[Index("Contitnent", Name = "ATR_HEAD_index_0")]
[Index("Contitnent", "SubContitnent", Name = "ATR_HEAD_index_1")]
public partial class AtrHead
{
    [Key]
    [Column("ATR_KEY")]
    public Guid AtrKey { get; set; }

    [Column("DESCRIPTION")]
    public string? Description { get; set; }

    [Column("CONTITNENT")]
    [StringLength(5)]
    public string? Contitnent { get; set; }

    [Column("SUB_CONTITNENT")]
    [StringLength(5)]
    public string? SubContitnent { get; set; }

    [Column("COUNTRY")]
    [StringLength(50)]
    public string? Country { get; set; }

    [Column("PIC_KEY")]
    public Guid? PicKey { get; set; }

    [InverseProperty("AtrKeyNavigation")]
    public virtual ICollection<AtrContent> AtrContents { get; set; } = new List<AtrContent>();

    [InverseProperty("AtrKeyNavigation")]
    public virtual ICollection<DesHead> DesHeads { get; set; } = new List<DesHead>();

    [InverseProperty("AtrKeyNavigation")]
    public virtual ICollection<PlcHead> PlcHeads { get; set; } = new List<PlcHead>();
}
