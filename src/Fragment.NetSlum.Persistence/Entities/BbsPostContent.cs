using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Persistence.Entities;

public class BbsPostContent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ushort Id { get; set; }

    public int PostId { get; set; }
    public BbsPost Post { get; set; } = default!;

    public string Content { get; set; } = "";
}
