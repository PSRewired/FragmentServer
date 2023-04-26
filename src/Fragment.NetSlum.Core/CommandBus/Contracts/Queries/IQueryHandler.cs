using Fragment.NetSlum.Core.CommandBus.Contracts.Commands;

namespace Fragment.NetSlum.Core.CommandBus.Contracts.Queries;

public interface IQueryHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    public Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}
