using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Core.Entities;

[Table("bbs_post_body")]
[Index("PostId", Name = "BBS_Post_Body_BBS_Post_Meta_postID_fk")]
[Index("PostBodyId", Name = "BBS_Post_Body_postBodyID_uindex", IsUnique = true)]
[MySqlCharSet("sjis")]
[MySqlCollation("sjis_japanese_ci")]
public class BbsPostBody
{
    [Key]
    [Column("postBodyID")]
    public int PostBodyId { get; set; }

    [Column("postBody", TypeName = "blob")]
    public byte[] PostBody { get; set; } = null!;

    [Column("postID")]
    public int PostId { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("BbsPostBodies")]
    public virtual BbsPostMetum Post { get; set; } = null!;
}
