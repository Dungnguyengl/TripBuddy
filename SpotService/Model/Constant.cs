using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotService.Model;

[Keyless]
[Table("CONSTANT", Schema = "SPOT")]
public partial class Constant
{
    [Column("CONSTANT_CODE")]
    [StringLength(5)]
    public string? ConstantCode { get; set; }

    [Column("CONSTANT_TYPE")]
    [StringLength(30)]
    public string? ConstantType { get; set; }

    [Column("CONSTANT_NAME")]
    [StringLength(30)]
    public string? ConstantName { get; set; }
}
