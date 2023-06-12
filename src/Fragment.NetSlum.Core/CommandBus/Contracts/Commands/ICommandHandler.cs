using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Fragment.NetSlum.Core.CommandBus.Contracts.Commands;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    public Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand {}
