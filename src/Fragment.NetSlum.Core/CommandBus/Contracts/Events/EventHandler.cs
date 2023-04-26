using MediatR;

namespace Fragment.NetSlum.Core.CommandBus.Contracts.Events;

/// <summary>
/// Abstract class used as an intermediary implementation, thus avoiding a vendor-lock on a third-party command bus library.
/// Currently both our implementation and MediatR use the same function declaration, so the integration is seamless.
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public abstract class EventHandler<TEvent> : IEventHandler<TEvent>, INotificationHandler<TEvent>
    where TEvent : IEvent
{
    public abstract Task Handle(TEvent eventInfo, CancellationToken cancellationToken);
}
