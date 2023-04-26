using Fragment.NetSlum.Core.CommandBus.Contracts.Commands;
using Fragment.NetSlum.Core.CommandBus.Contracts.Events;
using Fragment.NetSlum.Core.CommandBus.Contracts.Queries;

namespace Fragment.NetSlum.Core.CommandBus;

/// <summary>
/// Represents a command bus implementation. Note, if your extension does not support dependency injection scopes, you will
/// need to implement that yourself.
/// </summary>
public interface ICommandBus
{
    Task Notify<TEvent>(TEvent eventInfo, CancellationToken cancellationToken = default) where TEvent : IEvent;
    Task<TResult> GetResult<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>;
    Task<TResult> Execute<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>;
}
