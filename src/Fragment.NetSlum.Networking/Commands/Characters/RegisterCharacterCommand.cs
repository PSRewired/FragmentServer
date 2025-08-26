using Fragment.NetSlum.Core.CommandBus.Contracts.Commands;
using Fragment.NetSlum.Core.Models;
using Fragment.NetSlum.Persistence.Entities;

namespace Fragment.NetSlum.Networking.Commands.Characters;

public class RegisterCharacterCommand : ICommand<Character>
{
    public int PlayerAccountId { get; }
    public CharacterInfo CharacterInfo { get; }

    public RegisterCharacterCommand(int playerAccountId, CharacterInfo characterInfo)
    {
        PlayerAccountId = playerAccountId;
        CharacterInfo = characterInfo;
    }
}
