using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Core.Entities;

[Table("news_section_log")]
[Index("Id", Name = "news_section_log_id_uindex", IsUnique = true)]
[Index("SaveId", Name = "news_section_log_savedId_index")]
public class NewsSectionLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("saveId")]
    [StringLength(100)]
    public string SaveId { get; set; } = null!;

    [Column("articleId")]
    public ushort ArticleId { get; set; }
}
