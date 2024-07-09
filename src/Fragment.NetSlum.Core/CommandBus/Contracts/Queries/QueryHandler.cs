using System.Threading;
using System.Threading.Tasks;
using Mediator;

namespace Fragment.NetSlum.Core.CommandBus.Contracts.Queries;

/// <summary>
/// Abstract class used as an intermediary implementation, thus avoiding a vendor-lock on a third-party command bus library.
/// Currently both our implementation and MediatR use the same function declaration, so the integration is seamless.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
public abstract class QueryHandler<TCommand, TResult> : IQueryHandler<TCommand, TResult>, IRequestHandler<TCommand, TResult>
    where TCommand : IQuery<TResult>
{
    public abstract ValueTask<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}
