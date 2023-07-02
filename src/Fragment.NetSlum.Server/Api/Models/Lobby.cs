using System.Collections.Generic;
using Fragment.NetSlum.Core.Constants;

namespace Fragment.NetSlum.Server.Api.Models;

public class Lobby
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ushort PlayerCount { get; set; }
    public ChatLobbyType Type { get; set; }
    public ICollection<LobbyPlayer> Players { get; set; } = new List<LobbyPlayer>();
}
