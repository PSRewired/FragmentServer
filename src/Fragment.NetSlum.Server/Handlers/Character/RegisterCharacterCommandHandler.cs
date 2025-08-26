using System;
using System.Collections.Generic;
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

        var character = await _database.Characters
            .Include(c => c.PlayerAccount)
            .Include(c => c.CharacterStats)
            .Include(c => c.Guild)
            .Where(c => characterInfo.CharacterName == c.CharacterName &&
                        c.PlayerAccount != null && c.SaveId == characterInfo.SaveId)
            .FirstOrDefaultAsync(cancellationToken);

        var playerAccount = await _database.PlayerAccounts.FirstOrDefaultAsync(p => p.Id == command.PlayerAccountId, cancellationToken);

        if (playerAccount == null)
        {
            throw new KeyNotFoundException($"Could not find associated player account for player ID of {command.PlayerAccountId}");
        }

        character = CharacterInfoMapper.MapOrCreate(characterInfo, character);

        character.PlayerAccount = playerAccount;
        character.LastLoginAt = DateTime.UtcNow;

        _database.Update(character);
        await _database.SaveChangesAsync(cancellationToken);

        return character;
    }
}
