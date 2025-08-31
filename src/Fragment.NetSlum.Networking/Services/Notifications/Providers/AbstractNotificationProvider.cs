using System.Threading.Tasks;

namespace Fragment.NetSlum.Networking.Services.Notifications.Providers;

public interface INotificationProvider
{
    public Task Send(NotificationService.NotificationMessage message, object options);
}

public abstract class AbstractNotificationProvider<TOptions> : INotificationProvider where TOptions : BaseNotificationOptions
{
    protected abstract Task SendNotification(NotificationService.NotificationMessage message, TOptions options);

    public async Task Send(NotificationService.NotificationMessage message, object options)
    {
        await SendNotification(message, (TOptions)options);
    }
}
