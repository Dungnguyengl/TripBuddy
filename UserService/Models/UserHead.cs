using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UserService.Models;

[Table("USER_HEAD", Schema = "USER")]
[Index("FirstName", "LastName", Name = "USER_HEAD_index_0")]
public partial class UserHead
{
    [Key]
    [Column("USER_KEY")]
    public Guid UserKey { get; set; }

    [Column("FIRST_NAME")]
    [StringLength(20)]
    public string? FirstName { get; set; }

    [Column("LAST_NAME")]
    [StringLength(20)]
    public string? LastName { get; set; }

    [Column("EMAIL")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("PHONE_NUMBER")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Column("LOCATION_KEY")]
    public Guid? LocationKey { get; set; }

    [Column("LOCATION_NAME")]
    [StringLength(255)]
    public string? LocationName { get; set; }

    [Column("DOB")]
    public DateTime? Dob { get; set; }

    [Column("PIC_KEY")]
    public Guid? PicKey { get; set; }

    [Column("IS_ACTIVIE")]
    public bool? IsActivie { get; set; }
}
