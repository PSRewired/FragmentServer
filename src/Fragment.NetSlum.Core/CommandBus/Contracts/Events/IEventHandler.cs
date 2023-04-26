namespace Fragment.NetSlum.Core.CommandBus.Contracts.Events;

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    public Task Handle(TEvent eventInfo, CancellationToken cancellationToken);
}
