using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Persistence.Entities.Deprecated;

/// <summary>
/// type:
/// 1 = news
/// 2 = current maint
/// 3 = planned maint
/// 4 = past maint
/// </summary>
[Table("news")]
public class News
{
    [Key]
    [Column("key")]
    public int Key { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime Date { get; set; }

    [Column(TypeName = "tinytext")]
    public string Title { get; set; } = null!;

    [Column("URL", TypeName = "tinytext")]
    public string? Url { get; set; }

    public sbyte Type { get; set; }
}
