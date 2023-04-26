using Fragment.NetSlum.Core.CommandBus.Contracts.Commands;
using Fragment.NetSlum.Core.CommandBus.Contracts.Events;
using Fragment.NetSlum.Core.CommandBus.Contracts.Queries;
using MediatR;

namespace Fragment.NetSlum.Core.CommandBus;

/// <summary>
/// MediatR shim for the command bus
/// </summary>
public class MediatorCommandBus : ICommandBus
{
    private readonly IMediator _mediator;

    public MediatorCommandBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Notify<TEvent>(TEvent eventInfo, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
        await _mediator.Publish(eventInfo, cancellationToken);
    }

    public async Task<TResult> GetResult<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>
    {
        return await _mediator.Send(query, cancellationToken);
    }

    public async Task<TResult> Execute<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResult>
    {
        return await _mediator.Send(command, cancellationToken);
    }
}
