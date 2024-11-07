using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SpotService.Model;

[PrimaryKey("AtrKey", "DesKey", "PlcKey")]
[Table("EVL_PLC", Schema = "EVALUATE")]
public partial class EvlPlc
{
    [Key]
    [Column("ATR_KEY")]
    public Guid AtrKey { get; set; }

    [Key]
    [Column("DES_KEY")]
    public Guid DesKey { get; set; }

    [Key]
    [Column("PLC_KEY")]
    public Guid PlcKey { get; set; }

    [Column("ATR_NAME")]
    [StringLength(20)]
    public string? AtrName { get; set; }

    [Column("DES_NAME")]
    [StringLength(20)]
    public string? DesName { get; set; }

    [Column("PLC_NAME")]
    [StringLength(20)]
    public string? PlcName { get; set; }

    [Column("AVERAGE_RATE")]
    public double? AverageRate { get; set; }

    [Column("PIC_KEY")]
    public Guid PicKey { get; set; }

    [Column("PLC_DESCRIPTION")]
    [StringLength(255)]
    public string PlcDescription { get; set; } = null!;
}
