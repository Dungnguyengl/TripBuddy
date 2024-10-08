using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotService.Model;

[Table("ATR_CONTENT", Schema = "SPOT")]
[Index("AtrKey", "DesKey", Name = "ATR_CONTENT_index_4")]
public partial class AtrContent
{
    [Key]
    [Column("CONTENT_KEY")]
    public Guid ContentKey { get; set; }

    [Column("ATR_KEY")]
    public Guid AtrKey { get; set; }

    [Column("DES_KEY")]
    public Guid DesKey { get; set; }

    [Column("CONTENT_TYPE")]
    [StringLength(5)]
    public string? ContentType { get; set; }

    [Column("TITLE")]
    [StringLength(100)]
    public string? Title { get; set; }

    [Column("CONTENT")]
    public string? Content { get; set; }

    [ForeignKey("AtrKey")]
    [InverseProperty("AtrContents")]
    public virtual AtrHead AtrKeyNavigation { get; set; } = null!;

    [ForeignKey("DesKey")]
    [InverseProperty("AtrContents")]
    public virtual DesHead DesKeyNavigation { get; set; } = null!;
}
