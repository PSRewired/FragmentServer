using System;

namespace Fragment.NetSlum.Server.Api.Models;

public class GuildInfo
{
    public string Name { get; set; } = default!;
    public string Comment { get; set; } = default!;
    public int LeaderId { get; set; }
    public int GoldAmount { get; set; }
    public int SilverAmount { get; set; }
    public int BronzeAmount { get; set; }
    public int CurrentGp { get; set; }
    public int MemberCount { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}
