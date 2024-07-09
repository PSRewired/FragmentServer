using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus.Contracts.Events;
using Mediator;

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

    public async Task<TResult> GetResult<TResult>(Contracts.Queries.IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(query, cancellationToken);
    }

    public async Task<TResult> Execute<TResult>(Contracts.Commands.ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(command, cancellationToken);
    }
}
