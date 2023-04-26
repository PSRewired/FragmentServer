using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Entities.Deprecated;

[Keyless]
[Table("failed_jobs")]
public class FailedJob
{
    [Column("id")]
    public ulong? Id { get; set; }

    [Column("connection", TypeName = "text")]
    public string Connection { get; set; } = null!;

    [Column("queue", TypeName = "text")]
    public string Queue { get; set; } = null!;

    [Column("payload")]
    public string Payload { get; set; } = null!;

    [Column("exception")]
    public string Exception { get; set; } = null!;

    [Column("failed_at", TypeName = "timestamp")]
    public DateTime FailedAt { get; set; }
}
