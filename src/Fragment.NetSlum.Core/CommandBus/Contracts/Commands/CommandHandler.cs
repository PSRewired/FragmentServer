using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Fragment.NetSlum.Core.CommandBus.Contracts.Commands;

/// <summary>
/// Abstract class used as an intermediary implementation, thus avoiding a vendor-lock on a third-party command bus library
/// Currently both our implementation and MediatR use the same function declaration, so the integration is seamless.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResult"></typeparam>
public abstract class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>, IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    public abstract Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}
