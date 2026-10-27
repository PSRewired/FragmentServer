using System;
using System.Globalization;
using System.Linq;
using System.Threading;
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

    public override Task<int>ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        // AnsiConsole.Progress()
        //     .AutoClear(false)
        //     .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(),
        //         new SpinnerColumn())
        //     .Start(MigratePlayerRecords);

        // AnsiConsole.Progress()
        //     .AutoClear(false)
        //     .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(),
        //         new SpinnerColumn())
        //     .Start(MigrateCharacterRecords);

        AnsiConsole.Progress()
            .AutoClear(false)
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(),
                new SpinnerColumn())
            .Start(MigrateCharacterStatHistory);


        return Task.FromResult(0);
    }

    private void MigrateCharacterRecords(ProgressContext ctx)
    {
        var existingCharacters = _oldDatabase.Characterrepositories
            .OrderByDescending(c => c.PlayerId)
            .Skip(800);

        var characterCount = existingCharacters.Count();
        var pTask = ctx.AddTask($"[bold yellow] Generating {characterCount} character records[/]", maxValue: characterCount);

        var transaction = _database.Database.BeginTransaction();
        var count = 0;
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
                AnsiConsole.MarkupLine("[orange3]Unable to find old player record ID {0} for character {1}. Was the table migrated?[/]",
                    existingCharacter.AccountId!, existingCharacter.CharacterName!.AsSpan().ToShiftJisString());
                pTask.Increment(1);
                continue;
            }

            var newPlayerRecord = _database.PlayerAccounts.FirstOrDefault(p => p.SaveId == oldPlayerRecord.Saveid);

            if (newPlayerRecord == null)
            {
                AnsiConsole.MarkupLine("[orange3]Unable to find new player record ID {0} for character {1}. Was the table migrated?[/]",
                    existingCharacter.AccountId!, existingCharacter.CharacterName!.AsSpan().ToShiftJisString());
                pTask.Increment(1);
                continue;
            }

            var lastLogin = _oldDatabase.RankingData
                .AsNoTracking()
                .Where(r => r.CharacterSaveId == existingCharacter.CharacterSaveId)
                .OrderByDescending(r => r.Id)
                .FirstOrDefault();

            var firstLogin = _oldDatabase.RankingData
                .AsNoTracking()
                .Where(r => r.CharacterSaveId == existingCharacter.CharacterSaveId)
                .OrderBy(r => r.Id)
                .FirstOrDefault();

            var mappedCharacter = new Character
            {
                Id = existingCharacter.PlayerId,
                SaveId = existingCharacter.CharacterSaveId!.TrimNull(),
                CharacterName = RemoveGarbageNameCharacters(existingCharacter.CharacterName.AsSpan().ToShiftJisString().TrimNull()),
                Class = (CharacterClass)existingCharacter.ClassId!,
                CurrentLevel = existingCharacter.CharacterLevel!.Value,
                GreetingMessage = RemoveGarbageNameCharacters(existingCharacter.Greeting.AsSpan().ToShiftJisString().Replace("\r\n", "\n").TrimNull()),
                FullModelId = (uint)existingCharacter.ModelNumber!.Value,
                LastLoginAt = lastLogin != null
                    ? TryParseJankTimestamp(lastLogin.LoginTime)
                    : DateTime.MinValue,
                CreatedAt = firstLogin != null
                    ? TryParseJankTimestamp(firstLogin.LoginTime)
                    : DateTime.MinValue,
                CharacterStats = new CharacterStats
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

                count += 1;
                _database.SaveChanges();

                if (count % 100 == 0)
                {
                    transaction.Commit();
                    transaction.Dispose();
                    _database.ChangeTracker.Clear();

                    transaction = _database.Database.BeginTransaction();
                    AnsiConsole.WriteLine($"Persisting {count} character updates");
                }
            }
            catch (Exception e)
            {
                AnsiConsole.WriteLine($"{mappedCharacter.Id} -> {mappedCharacter.CharacterName} -> {mappedCharacter.GreetingMessage}");
                AnsiConsole.WriteLine($"{existingCharacter.Greeting!.ToHexDump()}");
                AnsiConsole.WriteException(e);
                pTask.Increment(1);

                continue;
            }

            pTask.Increment(1);
        }
    }

    private void MigrateCharacterStatHistory(ProgressContext ctx)
    {
        var existingStats = _oldDatabase.RankingData
            .AsNoTracking()
            .OrderBy(s => s.CharacterSaveId);

        var characterCount = existingStats.Count();
        var pTask = ctx.AddTask($"[bold yellow] Generating {characterCount} statistic records[/]", maxValue: characterCount);

        var count = 0;
        var currentAccountId = string.Empty;
        foreach (var stat in existingStats)
        {
            if (currentAccountId != stat.CharacterSaveId)
            {
                AnsiConsole.WriteLine($"AccountID changed! {currentAccountId} -> {stat.CharacterSaveId}");
                currentAccountId = stat.CharacterSaveId;
            }

            stat.CharacterSaveId = stat.CharacterSaveId.TrimNull();

            var existingCharacter = _database.Characters.AsNoTracking()
                .FirstOrDefault(c => c.SaveId.Equals(stat.CharacterSaveId));

            var existingCharacterId = existingCharacter?.Id;

            if (existingCharacterId == null)
            {
                AnsiConsole.WriteLine($"No character with name {stat.CharacterName} -- '{stat.CharacterSaveId}'");
                count += 1;
                pTask.Increment(1);
                continue;
            }

            var mappedCharacter = new CharacterStatHistory
            {
                Id = stat.Id,
                CharacterId = existingCharacterId!.Value,
                CurrentHp = stat.CharacterHp,
                CurrentGp = stat.CharacterGp,
                CurrentSp = stat.CharacterSp,
                CurrentLevel = existingCharacter!.CurrentLevel,
                AverageFieldLevel = stat.AverageFieldLevel,
                OnlineTreasures = (int)stat.GodStatueCounterOnline,
                CreatedAt = TryParseJankTimestamp(stat.LoginTime),
            };

            if (_database.CharacterStatHistory.AsNoTracking().Any(sh => sh.Id == mappedCharacter.Id))
            {
                _database.CharacterStatHistory.Update(mappedCharacter);
            }
            else
            {
                _database.CharacterStatHistory.Add(mappedCharacter);
            }

            count += 1;
            pTask.Increment(1);

            if (count % 50 == 0)
            {
                _database.SaveChanges();
                _database.ChangeTracker.Clear();
                AnsiConsole.WriteLine($"Saving {count} statistic records");
            }
        }

        _database.SaveChanges();
        _database.ChangeTracker.Clear();
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
                    SaveId = existingPlayer.Saveid.TrimNull(),
                };

                _database.Add(mappedPlayer);
                _database.SaveChanges();
                _database.ChangeTracker.Clear();
                pTask.Increment(1);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e, ExceptionFormats.ShortenEverything);
            }
        }

        AnsiConsole.MarkupLine("[green]Done updating players![/]");
    }

    private static DateTime TryParseJankTimestamp(string timestamp)
    {
        // The ranking database uses multiple different date formats for whatever reason so we need to perform some jank here to get the
        // right format.
        var loginTime = DateTime.MinValue;

        try
        {
            loginTime = DateTime.ParseExact(timestamp, "ddd MMM d HH:mm:ss yyyy", CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            try
            {
                loginTime = DateTime.ParseExact(timestamp, "ddd MMM  d HH:mm:ss yyyy", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }
        }

        return loginTime;
    }

    public static string RemoveGarbageNameCharacters(string input)
    {
        return input.Replace('', '?');
    }
}
