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

public class MigrateBbsCommand : AsyncCommand<MigrateBbsCommand.Settings>
{
    private readonly FragmentContext _database;
    private readonly OldFragmentContext _oldDatabase;

    public sealed class Settings : CommandSettings {}

    public MigrateBbsCommand(FragmentContext database, OldFragmentContext oldDatabase)
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
            .Start(MigrateBbs);

        return Task.FromResult(0);
    }

    private void MigrateBbs(ProgressContext ctx)
    {

        var existingBbs = _oldDatabase.BbsThreads.ToList();
        var guildCount = existingBbs.Count();

        var pTask = ctx.AddTask($"[bold yellow] Generating {guildCount} thread records[/]", maxValue: guildCount);

        foreach (var existingThread in existingBbs)
        {
            if (_database.BbsThreads.AsNoTracking().Any(g => g.Id == existingThread.ThreadId))
            {
                AnsiConsole.MarkupLine("[orchid]Thread with ID {0} already exists. Skipping...[/]", existingThread.ThreadId);
                pTask.Increment(1);
                continue;
            }

            var mappedThread = new BbsThread
            {
                Id = (ushort)existingThread.ThreadId,
                Title = existingThread.ThreadTitle.AsSpan().ToShiftJisString().TrimNull(),
                CategoryId = (ushort)existingThread.CategoryId,
            };

            _database.Add(mappedThread);

            var oldThreadPosts = _oldDatabase.BbsPostMeta
                .Where(pm => pm.ThreadId == existingThread.ThreadId)
                .ToList();

            var tTask = ctx.AddTask($"[bold cyan] Adding {oldThreadPosts.Count} posts to thread[/]", maxValue: oldThreadPosts.Count);

            foreach (var oldPost in oldThreadPosts)
            {
                var charName = oldPost.Username.AsSpan().ToShiftJisString();

                var oldContentRecord = _oldDatabase.BbsPostBodies
                    .AsNoTracking()
                    .FirstOrDefault(opb => opb.PostId == oldPost.PostId);

                var characterRecord = _database.Characters.FirstOrDefault(c => c.CharacterName == charName);

                if (characterRecord == null)
                {
                    AnsiConsole.MarkupLine("[bold red] Failed to import postId {0} because the character who created it does not exist[/]", oldPost.PostId);
                    continue;
                }

                var mappedPost = new BbsPost
                {
                    Id = oldPost.PostId,
                    Thread = mappedThread,
                    Title = oldPost.Title.AsSpan().ToShiftJisString().TrimNull(),
                    PostedBy = _database.Characters.First(c => c.CharacterName == charName),
                    CreatedAt = oldPost.Date,
                    PostContent = new BbsPostContent
                    {
                        Id = (ushort)(oldContentRecord?.PostBodyId ?? 0),
                        Content = oldContentRecord?.PostBody.AsSpan().ToShiftJisString().TrimNull() ?? "",
                    },
                };

                var postContent = new BbsPostContent
                {
                    Id = (ushort)(oldContentRecord?.PostBodyId ?? 0),
                    Post = mappedPost,
                    Content = oldContentRecord?.PostBody.AsSpan().ToShiftJisString().TrimNull() ?? "",
                };

                _database.Add(postContent);
                _database.Add(mappedPost);
                tTask.Increment(1);
            }

            _database.SaveChanges();
            _database.ChangeTracker.Clear();
            pTask.Increment(1);
        }
    }
}
