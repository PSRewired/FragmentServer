using Mediator;

namespace Fragment.NetSlum.Core.CommandBus.Contracts.Commands;

public interface ICommand<out TResult> : IRequest<TResult> { }

public interface ICommand : ICommand<Unit>, IRequest { }
