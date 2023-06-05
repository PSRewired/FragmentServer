using System;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Fragment.NetSlum.Console.Commands;

public class MigratePlayersCommand : AsyncCommand<MigratePlayersCommand.Settings>
{
    private readonly FragmentContext _database;
    private readonly OldFragmentContext _oldDatabase;

    public sealed class Settings : CommandSettings
    {
    }

    public MigratePlayersCommand(FragmentContext database, OldFragmentContext oldDatabase)
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
            .Start(MigratePlayerRecords);

        AnsiConsole.Progress()
            .AutoClear(false)
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(),
                new SpinnerColumn())
            .Start(MigrateCharacterRecords);


        return 0;
    }

    private void MigrateCharacterRecords(ProgressContext ctx)
    {
        var existingCharacters = _oldDatabase.Characterrepositories;
        var characterCount = existingCharacters.Count();
        var pTask = ctx.AddTask($"[bold yellow] Generating {characterCount} character records[/]", maxValue: characterCount);

        foreach (var existingCharacter in existingCharacters.ToList())
        {
            var exists = _database.Characters.AsNoTracking()
                .Include(c => c.CharacterStats)
                .FirstOrDefault(c => c.Id == existingCharacter.PlayerId);

            var oldPlayerRecord = _oldDatabase.PlayerAccountIds
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == existingCharacter.AccountId);

            if (oldPlayerRecord == null)
            {
                AnsiConsole.MarkupLine("[orange3]Unable to find old player record ID {0} for character {1}. Was the table migrated?[/]", existingCharacter.AccountId!, existingCharacter.CharacterName!.AsSpan().ToShiftJisString());
                pTask.Increment(1);
                continue;
            }

            var newPlayerRecord = _database.PlayerAccounts.FirstOrDefault(p => p.SaveId == oldPlayerRecord.Saveid);

            if (newPlayerRecord == null)
            {
                AnsiConsole.MarkupLine("[orange3]Unable to find new player record ID {0} for character {1}. Was the table migrated?[/]", existingCharacter.AccountId!, existingCharacter.CharacterName!.AsSpan().ToShiftJisString());
                pTask.Increment(1);
                continue;
            }

            var mappedCharacter = new Character
            {
                Id = existingCharacter.PlayerId,
                CharacterName = existingCharacter.CharacterName.AsSpan().ToShiftJisString(),
                Class = (CharacterClass)existingCharacter.ClassId!,
                CurrentLevel = existingCharacter.CharacterLevel!.Value,
                GreetingMessage = existingCharacter.Greeting.AsSpan().ToShiftJisString().Replace("\r\n", "\n"),
                FullModelId = (uint)existingCharacter.ModelNumber!.Value,
                CharacterStats = new()
                {
                    Id = exists?.CharacterStats.Id ?? 0,
                    CharacterId = existingCharacter.PlayerId,
                    CurrentHp = (int)existingCharacter.CharHp!,
                    CurrentSp = (int)existingCharacter.CharSp!,
                    CurrentGp = (uint)existingCharacter.CharGp!,
                    BronzeAmount = (int)existingCharacter.CharBronzeCoin!,
                    SilverAmount = (int)existingCharacter.CharSilverCoin!,
                    GoldAmount = (int)existingCharacter.CharGoldCoin!,
                    OnlineTreasures = (int)existingCharacter.CharOnlineGoat!,
                    AverageFieldLevel = (int)existingCharacter.CharOfflineGoat!,
                },
                PlayerAccount = newPlayerRecord,
            };

            try
            {
                if (exists != null)
                {
                    _database.Update(mappedCharacter);
                }
                else
                {
                    _database.Add(mappedCharacter);
                }
                _database.SaveChanges();
                _database.ChangeTracker.Clear();
            }
            catch (Exception e)
            {
                AnsiConsole.WriteLine($"{mappedCharacter.Id} -> {mappedCharacter.CharacterName} -> {mappedCharacter.GreetingMessage}");
                AnsiConsole.WriteLine($"{existingCharacter.Greeting!.ToHexDump()}");
                AnsiConsole.WriteException(e);
                continue;
            }
            pTask.Increment(1);
        }
    }

    private void MigratePlayerRecords(ProgressContext ctx)
    {
        var existingPlayers = _oldDatabase.PlayerAccountIds;
        var playerCount = existingPlayers.Count();

        var pTask = ctx.AddTask($"[bold yellow] Generating {playerCount} player records[/]", maxValue: playerCount);

        foreach (var existingPlayer in existingPlayers)
        {
            try
            {
                var playerWithExistingSaveId = _database.PlayerAccounts
                    .AsNoTracking()
                    .FirstOrDefault(p => p.SaveId == existingPlayer.Saveid);

                if (playerWithExistingSaveId != null)
                {
                    AnsiConsole.MarkupLine("[orange3]A player already exists with id {0} for SaveId {1}[/]", playerWithExistingSaveId.Id,
                        playerWithExistingSaveId.SaveId);
                    pTask.Increment(1);
                    continue;
                }

                var mappedPlayer = new PlayerAccount
                {
                    Id = (ushort)existingPlayer.Id,
                    SaveId = existingPlayer.Saveid,
                };

                _database.Add(mappedPlayer);
                pTask.Increment(1);

                if (pTask.Value % 100 != 0)
                {
                    continue;
                }

                AnsiConsole.MarkupLine("[cyan]Persisting 100 records to the database...[/]");
                _database.SaveChanges();
                _database.ChangeTracker.Clear();
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e, ExceptionFormats.ShortenEverything);
            }
        }

        AnsiConsole.MarkupLine("[cyan]Flushing all remaining records...[/]");
        _database.SaveChanges();
        _database.ChangeTracker.Clear();
        AnsiConsole.MarkupLine("[green]Done updating players![/]");
    }
}
