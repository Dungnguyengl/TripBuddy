using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SpotService.Model;

[Table("EVL_HEAD", Schema = "EVALUATE")]
public partial class EvlHead
{
    [Key]
    [Column("EVL_KEY")]
    public Guid EvlKey { get; set; }

    [Column("PLC_KEY")]
    public Guid? PlcKey { get; set; }

    [Column("EVL_TITLE")]
    [StringLength(100)]
    public string? EvlTitle { get; set; }

    [Column("EVL_CONTENT")]
    public string? EvlContent { get; set; }

    [Column("PIC_KEY")]
    public Guid? PicKey { get; set; }

    [Column("RATE")]
    public short? Rate { get; set; }
}
