using System;

namespace Fragment.NetSlum.Server.Api.Models;

public class AreaServerStatus
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = default!;
    public ushort Level { get; set; }
    public byte Status { get; set; }
    public byte State { get; set; }
    public ushort CurrentPlayerCount { get;set; }
    public DateTime OnlineSince { get; set; }
}
