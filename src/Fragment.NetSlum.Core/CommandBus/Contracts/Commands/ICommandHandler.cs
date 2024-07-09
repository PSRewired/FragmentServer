using System.Threading;
using System.Threading.Tasks;
using Mediator;

namespace Fragment.NetSlum.Core.CommandBus.Contracts.Commands;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    public ValueTask<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand {}
