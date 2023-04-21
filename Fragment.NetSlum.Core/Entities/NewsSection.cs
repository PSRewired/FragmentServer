using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Core.Entities;

[Table("news_section")]
[Index("ArticleId", Name = "news_section_articleID_uindex", IsUnique = true)]
public class NewsSection
{
    [Key]
    [Column("articleID")]
    public ushort ArticleId { get; set; }

    [Column("articleTitle")]
    [StringLength(33)]
    public string ArticleTitle { get; set; } = null!;

    [Column("articleBody")]
    [StringLength(412)]
    public string ArticleBody { get; set; } = null!;

    [Column("articleDate", TypeName = "datetime")]
    public DateTime ArticleDate { get; set; }

    [Column("articleImage", TypeName = "blob")]
    public byte[]? ArticleImage { get; set; }
}
