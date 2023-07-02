using System;

namespace Fragment.NetSlum.Server.Api.Models;

public class AreaServerStatus
{
    public string Name { get; set; } = default!;
    public ushort Level { get; set; }
    public byte State { get; set; }
    public ushort CurrentPlayerCount { get;set; }
    public DateTime OnlineSince { get; set; }
}
