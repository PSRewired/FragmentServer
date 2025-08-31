using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus.Contracts.Events;
using Fragment.NetSlum.Networking.Services.Notifications;

namespace Fragment.NetSlum.Networking.Events.Handlers;

public class AreaServerStatusUpdateEventHandler : EventHandler<AreaServerStatusUpdateEvent>
{
    private readonly NotificationService _notificationService;

    public AreaServerStatusUpdateEventHandler(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public override async ValueTask Handle(AreaServerStatusUpdateEvent eventInfo, CancellationToken cancellationToken)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Name: {eventInfo.ServerName}");
        sb.AppendLine($"Level: {eventInfo.Level}");
        sb.AppendLine($"State: {eventInfo.State.ToString()}");
        sb.AppendLine($"Player Count: {eventInfo.CurrentPlayerCount}");

        await _notificationService.SendNotification(new NotificationService.NotificationMessage(
            NotificationService.NotificationType.AreaServer, "Area Server Status Changed", sb.ToString()));
    }
}
