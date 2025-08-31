using System.Threading.Tasks;
using Discord;
using Discord.Webhook;

namespace Fragment.NetSlum.Networking.Services.Notifications.Providers.Discord;

public class DiscordNotificationProvider : AbstractNotificationProvider<DiscordNotificationOptions>
{
    protected override async Task SendNotification(NotificationService.NotificationMessage message, DiscordNotificationOptions options)
    {
        using var discordClient = new DiscordWebhookClient(options.WebhookUri);

        var embed = new EmbedBuilder
        {
            Title = message.Title,
            Description = message.Content,
        };

        await discordClient.SendMessageAsync(embeds: [embed.Build()]);
    }
}
