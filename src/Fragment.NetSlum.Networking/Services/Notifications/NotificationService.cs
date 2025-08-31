using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Services.Notifications.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fragment.NetSlum.Networking.Services.Notifications;

public class NotificationService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOptionsMonitor<NotificationOptions> _optionsMonitor;

    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
    };

    [Flags]
    public enum NotificationType
    {
        AreaServer = 1 << 0,
        Character = 1 << 1,
        All = AreaServer | Character,
    }

    public record NotificationMessage(NotificationType Type, string Title, string Content);

    public NotificationService(IServiceScopeFactory scopeFactory, IOptionsMonitor<NotificationOptions> optionsMonitor)
    {
        _scopeFactory = scopeFactory;
        _optionsMonitor = optionsMonitor;
    }

    public async Task SendNotification(NotificationMessage message)
    {
        using var scope = _scopeFactory.CreateScope();

        var configuredNotifications = _optionsMonitor.CurrentValue.Destinations
            .Where(n => n.Types.Any(t => t.HasFlag(message.Type)))
            .ToList();

        foreach (var destination in configuredNotifications)
        {
            var provider = scope.ServiceProvider.GetRequiredKeyedService<INotificationProvider>(destination.ProviderName);

            var optionsType = provider.GetType().BaseType!.GetGenericArguments()[0];

            var jsonOptions = JsonSerializer.Serialize(destination.Options, SerializerOptions);

            await provider.Send(message, JsonSerializer.Deserialize(jsonOptions, optionsType, SerializerOptions)!);
        }

    }
}
