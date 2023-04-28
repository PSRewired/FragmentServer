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
    Task<TResult> GetResult<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    Task<TResult> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
}
