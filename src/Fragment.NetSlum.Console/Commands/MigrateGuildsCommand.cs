using System;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Spectre.Console.Cli;

namespace Fragment.NetSlum.Console.Commands;

public class MigrateGuildsCommand : AsyncCommand<MigrateGuildsCommand.Settings>
{
    private readonly FragmentContext _database;
    private readonly OldFragmentContext _oldDatabase;

    public sealed class Settings : CommandSettings {}

    public MigrateGuildsCommand(FragmentContext database, OldFragmentContext oldDatabase)
    {
        _database = database;
        _oldDatabase = oldDatabase;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var existingGuilds = _oldDatabase.Guildrepositories;

        foreach (var existingGuild in existingGuilds)
        {
            var mappedGuild = new Guild
            {
                Id = (ushort)existingGuild.GuildId,
                Name = existingGuild.GuildName!.AsSpan().ToShiftJisString(),
                Comment = existingGuild.GuildComment!.AsSpan().ToShiftJisString(),
                Emblem = existingGuild.GuildEmblem!,
                Stats = new GuildStats
                {
                    GuildId = (ushort)existingGuild.GuildId,
                    BronzeAmount = existingGuild.BronzeCoin!.Value,
                    SilverAmount = existingGuild.SilverCoin!.Value,
                    GoldAmount = existingGuild.GoldCoin!.Value!,
                    CurrentGp = existingGuild.Gp!.Value,
                    CreatedAt = DateTime.Parse(existingGuild.EstablishmentDate!),
                },
                CreatedAt = DateTime.Parse(existingGuild.EstablishmentDate!),
            };

            _database.Add(mappedGuild);

            await _database.SaveChangesAsync();
            break;
        }

        return 0;
    }
}
