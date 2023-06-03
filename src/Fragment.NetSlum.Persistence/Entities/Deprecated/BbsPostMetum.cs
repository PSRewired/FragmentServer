using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities.Deprecated;

[Table("bbs_post_meta")]
[Index("ThreadId", Name = "BBS_Post_Meta_BBS_Threads_threadID_fk")]
[Index("PostId", Name = "BBS_Post_Meta_postID_uindex", IsUnique = true)]
[Index("Unk0", Name = "BBS_Post_Meta_unk0_uindex", IsUnique = true)]
[MySqlCharSet("cp932")]
[MySqlCollation("cp932_japanese_ci")]
public class BbsPostMetum
{
    [Column("unk0")]
    public int Unk0 { get; set; }

    [Key]
    [Column("postID")]
    public int PostId { get; set; }

    [Column("unk2")]
    public int Unk2 { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column("username", TypeName = "blob")]
    public byte[] Username { get; set; } = null!;

    [Column("subtitle", TypeName = "blob")]
    public byte[] Subtitle { get; set; } = null!;

    [Column("title", TypeName = "blob")]
    public byte[] Title { get; set; } = null!;

    [Column("unk3")]
    [StringLength(44)]
    [MySqlCharSet("utf8mb4")]
    [MySqlCollation("utf8mb4_0900_ai_ci")]
    public string Unk3 { get; set; } = null!;

    [Column("threadID")]
    public int ThreadId { get; set; }

    [InverseProperty("Post")]
    public virtual ICollection<BbsPostBody> BbsPostBodies { get; set; } = new List<BbsPostBody>();

    [ForeignKey("ThreadId")]
    [InverseProperty("BbsPostMeta")]
    public virtual BbsThread Thread { get; set; } = null!;
}
