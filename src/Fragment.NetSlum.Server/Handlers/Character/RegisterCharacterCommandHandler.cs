using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus.Contracts.Commands;
using Fragment.NetSlum.Networking.Commands.Characters;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Handlers.Character;

public class RegisterCharacterCommandHandler : CommandHandler<RegisterCharacterCommand, Persistence.Entities.Character>
{
    private readonly FragmentContext _database;

    public RegisterCharacterCommandHandler(FragmentContext database)
    {
        _database = database;
    }

    public override async ValueTask<Persistence.Entities.Character> Handle(RegisterCharacterCommand command, CancellationToken cancellationToken)
    {
        var characterInfo = command.CharacterInfo;

        Persistence.Entities.Character? character = await _database.Characters
            .Include(c => c.PlayerAccount)
            .Include(c => c.CharacterStats)
            .Include(c => c.Guild)
            .Where(c => characterInfo.CharacterName == c.CharacterName &&
                        c.PlayerAccount != null && c.PlayerAccount!.SaveId == characterInfo.SaveId)
            .FirstOrDefaultAsync(cancellationToken);

        PlayerAccount? playerAccount = await _database.PlayerAccounts.FirstOrDefaultAsync(p => p.SaveId == characterInfo.SaveId, cancellationToken);

        // If the player account is missing, attempt to create it now.
        if (playerAccount == null)
        {
            playerAccount = new PlayerAccount
            {
                SaveId = characterInfo.SaveId,
            };
        }

        character = CharacterInfoMapper.MapOrCreate(characterInfo, character);

        character.PlayerAccount = playerAccount;
        character.LastLoginAt = DateTime.UtcNow;

        _database.Update(character);
        await _database.SaveChangesAsync(cancellationToken);

        return character;
    }
}
