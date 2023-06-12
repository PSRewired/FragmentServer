using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Core.CommandBus.Contracts.Commands;
using Fragment.NetSlum.Networking.Commands.News;
using Fragment.NetSlum.Networking.Queries.News;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;

namespace Fragment.NetSlum.Server.Handlers.News;

public class MarksNewsArticleReadCommandHandler : CommandHandler<MarkNewsArticleReadCommand, bool>
{
    private readonly FragmentContext _database;
    private readonly ICommandBus _commandBus;

    public MarksNewsArticleReadCommandHandler(FragmentContext database, ICommandBus commandBus)
    {
        _database = database;
        _commandBus = commandBus;
    }

    public override async Task<bool> Handle(MarkNewsArticleReadCommand command, CancellationToken cancellationToken)
    {
        if (await _commandBus.GetResult(new HasPlayerReadNewsArticle(command.PlayerId, command.ArticleId), cancellationToken))
        {
            return true;
        }

        _database.WebNewsReadLogs.Add(new WebNewsReadLog
        {
            PlayerAccountId = command.PlayerId,
            WebNewsArticleId = command.ArticleId,
        });

        await _database.SaveChangesAsync(cancellationToken);

        return true;
    }
}
