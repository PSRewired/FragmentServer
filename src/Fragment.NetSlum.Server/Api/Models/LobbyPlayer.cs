using System;

namespace Fragment.NetSlum.Server.Api.Models;

public class LobbyPlayer
{
    public int CharacterId { get; set; }
    public string CharacterName { get; set; } = default!;
    public DateTime JoinedAt { get; set; }
}
