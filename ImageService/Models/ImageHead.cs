using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ImageService.Models;

[Table("IMAGE_HEAD", Schema = "IMAGE")]
public partial class ImageHead
{
    [Key]
    [Column("IMAGE_KEY")]
    public Guid ImageKey { get; set; }

    [Column("IS_DELETE")]
    public bool? IsDelete { get; set; }

    [Column("DELETE_DATE")]
    public DateTime? DeleteDate { get; set; }

    [Column("IMAGE_TYPE")]
    [StringLength(20)]
    public string? ImageType { get; set; }
}
