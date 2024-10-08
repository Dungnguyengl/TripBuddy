using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripperService.Model;

[Keyless]
[Table("TRIP_PLC", Schema = "TRIPPER")]
public partial class TripPlc
{
    [Column("TRIP_KEY")]
    public Guid? TripKey { get; set; }

    [Column("REFERENT_KEY")]
    public Guid? ReferentKey { get; set; }

    [Column("PLC_TYPE")]
    [StringLength(255)]
    public string PlcType { get; set; } = null!;

    [Column("PLC_CONTENT")]
    [StringLength(100)]
    public string? PlcContent { get; set; }

    [ForeignKey("TripKey")]
    public virtual TripHead? TripKeyNavigation { get; set; }
}
