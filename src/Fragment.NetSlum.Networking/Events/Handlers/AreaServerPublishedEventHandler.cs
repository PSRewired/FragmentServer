using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus.Contracts.Events;
using Fragment.NetSlum.Networking.Services.Notifications;

namespace Fragment.NetSlum.Networking.Events.Handlers;

public class AreaServerPublishedEventHandler : EventHandler<AreaServerPublishedEvent>
{
    private readonly NotificationService _notificationService;

    public AreaServerPublishedEventHandler(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public override async ValueTask Handle(AreaServerPublishedEvent eventInfo, CancellationToken cancellationToken)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Name: {eventInfo.AreaServerInfo.ServerName}");
        sb.AppendLine($"Level: {eventInfo.AreaServerInfo.Level}");

        await _notificationService.SendNotification(new NotificationService.NotificationMessage(
            NotificationService.NotificationType.AreaServer, "Area Server Published", sb.ToString()));
    }
}
