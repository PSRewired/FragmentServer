using System.Threading;
using System.Threading.Tasks;

namespace Fragment.NetSlum.Core.CommandBus.Contracts.Events;

internal interface IEventHandler<in TEvent> where TEvent : IEvent
{
    public ValueTask Handle(TEvent eventInfo, CancellationToken cancellationToken);
}
