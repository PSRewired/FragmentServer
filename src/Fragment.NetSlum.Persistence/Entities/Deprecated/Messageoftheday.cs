using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities.Deprecated;

[Table("messageoftheday")]
[MySqlCharSet("sjis")]
[MySqlCollation("sjis_japanese_ci")]
public class Messageoftheday
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(500)]
    [MySqlCharSet("utf8mb4")]
    [MySqlCollation("utf8mb4_0900_ai_ci")]
    public string Message { get; set; } = null!;
}
