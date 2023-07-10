using Fragment.NetSlum.Core.CommandBus.Contracts.Events;

namespace Fragment.NetSlum.Networking.Events;

/// <summary>
/// Event that is fired when a character is selected and active and/or registered with the server
/// </summary>
public class CharacterLoggedInEvent : IEvent
{
    public int CharacterId { get; set; }
    public string IpAddress { get; set; }

    public CharacterLoggedInEvent(int characterId, string ipAddress)
    {
        CharacterId = characterId;
        IpAddress = ipAddress;
    }
}
