using System;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
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
        AnsiConsole.Progress()
            .AutoClear(false)
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(),
                new SpinnerColumn())
            .Start(MigrateGuilds);

        AnsiConsole.Progress()
            .AutoClear(false)
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(),
                new SpinnerColumn())
            .Start(MigrateGuildMembers);

        return 0;
    }

    private void MigrateGuilds(ProgressContext ctx)
    {

        var existingGuilds = _oldDatabase.Guildrepositories;
        var guildCount = existingGuilds.Count();

        var pTask = ctx.AddTask($"[bold yellow] Generating {guildCount} guild records[/]", maxValue: guildCount);

        foreach (var existingGuild in existingGuilds)
        {
            if (_database.Guilds.AsNoTracking().Any(g => g.Id == existingGuild.GuildId))
            {
                AnsiConsole.MarkupLine("[orchid]Guild with ID {0} already exists. Skipping...[/]", existingGuild.GuildId);
                pTask.Increment(1);
                continue;
            }

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
                Leader = _database.Characters.FirstOrDefault(c => c.Id == existingGuild.MasterPlayerId!.Value)
            };

            _database.Add(mappedGuild);
            _database.SaveChanges();
            _database.ChangeTracker.Clear();
            pTask.Increment(1);
        }
    }

    private void MigrateGuildMembers(ProgressContext ctx)
    {
        var existingGuilds = _oldDatabase.Guildrepositories;
        var guildCount = existingGuilds.Count();

        var pTask = ctx.AddTask($"[bold yellow] Updating guild member records[/]", maxValue: guildCount);

        foreach (var existingGuild in existingGuilds.ToList())
        {
            var newGuild = _database.Guilds.First(g => g.Id == existingGuild.GuildId);

            var oldGuildMemberIds = _oldDatabase.Characterrepositories
                .AsNoTracking()
                .Where(c => c.GuildId == newGuild.Id)
                .Select(m => m.PlayerId)
                .ToArray();

            var newMembers = _database.Characters.Where(p => oldGuildMemberIds.Contains(p.Id));
            var newMemberCount = newMembers.Count();

            var gTask = ctx.AddTask($"Adding {newMemberCount} members for guild", maxValue: newMemberCount);

            foreach (var member in newMembers)
            {
                newGuild.Members.Add(member);
                gTask.Increment(1);
            }

            AnsiConsole.MarkupLine("[cyan] Adding {0} members to guild ({1}) {2}[/]", newMembers.Count(), newGuild.Id, newGuild.Name);

            _database.SaveChanges();
            _database.ChangeTracker.Clear();
            pTask.Increment(1);
        }
    }
}
