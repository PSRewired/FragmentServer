using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Core.Entities;

[Table("bbs_threads")]
[Index("CategoryId", Name = "BBS_Threads_BBS_Category_categoryID_fk")]
[Index("ThreadId", Name = "BBS_Threads_threadID_uindex", IsUnique = true)]
[MySqlCharSet("sjis")]
[MySqlCollation("sjis_japanese_ci")]
public class BbsThread
{
    [Key]
    [Column("threadID")]
    public int ThreadId { get; set; }

    [Column("threadTitle", TypeName = "blob")]
    public byte[] ThreadTitle { get; set; } = null!;

    [Column("categoryID")]
    public int CategoryId { get; set; }

    [InverseProperty("Thread")]
    public virtual ICollection<BbsPostMetum> BbsPostMeta { get; set; } = new List<BbsPostMetum>();

    [ForeignKey("CategoryId")]
    [InverseProperty("BbsThreads")]
    public virtual BbsCategory Category { get; set; } = null!;
}
