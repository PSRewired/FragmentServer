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

public class MigrateGuildShopCommand : AsyncCommand<MigrateGuildShopCommand.Settings>
{
    private readonly FragmentContext _database;
    private readonly OldFragmentContext _oldDatabase;

    public sealed class Settings : CommandSettings {}

    public MigrateGuildShopCommand(FragmentContext database, OldFragmentContext oldDatabase)
    {
        _database = database;
        _oldDatabase = oldDatabase;
    }

    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.Progress()
            .AutoClear(false)
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(),
                new SpinnerColumn())
            .Start(MigrateGuildShop);

        return Task.FromResult(0);
    }

    private void MigrateGuildShop(ProgressContext ctx)
    {
        var existingGuildShop = _oldDatabase.Guilditemshops.ToList();
        var guildCount = existingGuildShop.Count();

        var pTask = ctx.AddTask($"[bold yellow] Creating or updating {guildCount} guild shop records[/]", maxValue: guildCount);

        foreach (var oldItem in existingGuildShop)
        {
            GuildShopItem? guildItem = _database.GuildShopItems.FirstOrDefault(gs => gs.Id == oldItem.ItemShopId);

            if (guildItem == null)
            {
                guildItem = new GuildShopItem();
                _database.GuildShopItems.Add(guildItem);
            }

            guildItem.Id = oldItem.ItemShopId;
            guildItem.GuildId = (ushort)oldItem.GuildId!.Value;
            guildItem.ItemId = oldItem.ItemId!.Value;
            guildItem.Quantity = (ushort)oldItem.Quantity!.Value;
            guildItem.Price = (uint)oldItem.GeneralPrice!.Value;
            guildItem.MemberPrice = (uint)oldItem.MemberPrice!.Value;
            guildItem.AvailableForGeneral = oldItem.AvailableForGeneral!.Value;
            guildItem.AvailableForMember = oldItem.AvailableForGeneral!.Value;

            pTask.Increment(1);

            if (pTask.Value % 100 != 0)
            {
                continue;
            }

            AnsiConsole.MarkupLine("[cyan]Persisting 100 records to the database...[/]");

            _database.SaveChanges();
            _database.ChangeTracker.Clear();
            pTask.Increment(1);
        }
    }
}
