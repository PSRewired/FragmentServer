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

public class MigrateMailCommand : AsyncCommand<MigrateMailCommand.Settings>
{
    private readonly FragmentContext _database;
    private readonly OldFragmentContext _oldDatabase;

    public sealed class Settings : CommandSettings {}

    public MigrateMailCommand(FragmentContext database, OldFragmentContext oldDatabase)
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
            .Start(MigrateMail);

        return Task.FromResult(0);
    }

    private void MigrateMail(ProgressContext ctx)
    {
        var existingMail = _oldDatabase.MailMeta.ToList();
        var mailCount = existingMail.Count;

        var pTask = ctx.AddTask($"[bold yellow] Generating {mailCount} mail records[/]", maxValue: mailCount);

        foreach (var mailMeta in existingMail)
        {
            if (_database.Mails.AsNoTracking().Any(g => g.Id == mailMeta.MailId))
            {
                AnsiConsole.MarkupLine("[orchid]Mail with ID {0} already exists. Skipping...[/]", mailMeta.MailId);
                pTask.Increment(1);
                continue;
            }

            var mappedContent = _oldDatabase.MailBodies.AsNoTracking()
                .Where(m => m.MailId == mailMeta.MailId)
                .Select(oldContent => new
                {
                    AvatarId = oldContent.FaceId,
                    Content = new MailContent
                    {
                        Id = oldContent.MailBodyId,
                        Content = oldContent.MailBody1.AsMemory().ToShiftJisString().TrimNull(),
                        MailId = oldContent.MailId,
                    }
                })
                .FirstOrDefault();

            var mappedMail = new Mail
            {
                Id = mailMeta.MailId,
                CreatedAt = mailMeta.Date,
                Recipient = _database.PlayerAccounts.FirstOrDefault(pa => pa.Id == mailMeta.ReceiverAccountId),
                RecipientName = mailMeta.ReceiverName.AsSpan().ToShiftJisString().TrimNull(),
                Sender = _database.PlayerAccounts.FirstOrDefault(pa => pa.Id == mailMeta.SenderAccountId),
                SenderName = mailMeta.SenderName.AsSpan().ToShiftJisString().TrimNull(),
                Read = true, // This field does not exist on the current database, default it to true for the migration
                Delivered = mailMeta.MailDelivered!.Value,
                AvatarId = mappedContent?.AvatarId ?? "",
                Content = mappedContent?.Content,
                Subject = mailMeta.MailSubject.AsSpan().ToShiftJisString().TrimNull(),
            };

            _database.Add(mappedMail);

            _database.SaveChanges();
            _database.ChangeTracker.Clear();
            pTask.Increment(1);
        }
    }
}
