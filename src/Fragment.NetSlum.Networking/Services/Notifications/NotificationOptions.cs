using System.Collections.Generic;

namespace Fragment.NetSlum.Networking.Services.Notifications;

public class NotificationOptions
{
    public class NotificationDestination
    {
        public required string ProviderName { get; set; }
        public Dictionary<string, object?> Options { get; set; } = new Dictionary<string, object?>();

        public ICollection<NotificationService.NotificationType> Types { get; set; } = [NotificationService.NotificationType.All];
    }

    public ICollection<NotificationDestination> Destinations { get; set; } = [];
}
