using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotService.Model;

[Table("PLC_HEAD", Schema = "SPOT")]
[Index("AtrKey", "DesKey", Name = "PLC_HEAD_index_3")]
public partial class PlcHead
{
    [Key]
    [Column("PLC_KEY")]
    public Guid PlcKey { get; set; }

    [Column("ATR_KEY")]
    public Guid AtrKey { get; set; }

    [Column("DES_KEY")]
    public Guid DesKey { get; set; }

    [Column("PLC_NAME")]
    [StringLength(50)]
    public string? PlcName { get; set; }

    [Column("DESCRIPTION")]
    public string? Description { get; set; }

    [Column("LONGITUDE")]
    public double? Longitude { get; set; }

    [Column("LATITUDE")]
    public double? Latitude { get; set; }

    [Column("PIC_KEY")]
    public Guid? PicKey { get; set; }

    [ForeignKey("AtrKey")]
    [InverseProperty("PlcHeads")]
    public virtual AtrHead AtrKeyNavigation { get; set; } = null!;

    [ForeignKey("DesKey")]
    [InverseProperty("PlcHeads")]
    public virtual DesHead DesKeyNavigation { get; set; } = null!;
}
