using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TripperService.Model;

[Table("TRIP_HEAD", Schema = "TRIPPER")]
[Index("StartDate", Name = "TRIP_HEAD_index_0")]
[Index("StartDate", "EndDate", Name = "TRIP_HEAD_index_1")]
public partial class TripHead
{
    [Key]
    [Column("TRIP_KEY")]
    public Guid TripKey { get; set; }

    [Column("START_DATE")]
    public DateTime? StartDate { get; set; }

    [Column("END_DATE")]
    public DateTime? EndDate { get; set; }
}
