using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UserService.Models;

[Keyless]
[Table("USER_SPOT", Schema = "USER")]
[Index("UserKey", Name = "UQ__USER_SPO__5F13FD3DF93FA2B4", IsUnique = true)]
[Index("UserKey", Name = "USER_SPOT_index_1", IsUnique = true)]
public partial class UserSpot
{
    [Column("USER_KEY")]
    public Guid UserKey { get; set; }

    [Column("REWARD_POINT")]
    public long? RewardPoint { get; set; }

    [Column("NO_TRIP")]
    public long? NoTrip { get; set; }

    [Column("NO_PLACE")]
    public long? NoPlace { get; set; }

    [Column("RATING")]
    public short? Rating { get; set; }
}
