using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fragment.NetSlum.Persistence.Entities;

public class BbsPost
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int ThreadId { get; set; }
    public BbsThread Thread { get; set; } = default!;

    public int PostedById { get; set; }
    public Character PostedBy { get; set; } = default!;

    public required string Title { get; set; }
    public string Subtitle { get; set; } = "";

    public int ContentId { get; set; }
    public BbsPostContent Content { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
