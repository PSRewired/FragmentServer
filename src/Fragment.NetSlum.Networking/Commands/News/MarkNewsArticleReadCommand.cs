using Fragment.NetSlum.Core.CommandBus.Contracts.Commands;

namespace Fragment.NetSlum.Networking.Commands.News;

public class MarkNewsArticleReadCommand : ICommand, ICommand<bool>
{
    public int PlayerId { get; }
    public ushort ArticleId { get; }

    public MarkNewsArticleReadCommand(ushort articleId, int playerId)
    {
        ArticleId = articleId;
        PlayerId = playerId;
    }
}
