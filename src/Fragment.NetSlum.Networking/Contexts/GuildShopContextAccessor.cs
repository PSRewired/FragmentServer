namespace Fragment.NetSlum.Networking.Contexts;

/// <summary>
/// Stores contextual information about actions happening in a guild shop for a session
/// </summary>
public class GuildShopContextAccessor
{
    public record GuildShopItemDonation(uint ToGuildId, uint ItemId, ushort Quantity);

    public struct GuildShopContext
    {
        public GuildShopItemDonation? Donation { get; set; }
    }

    public GuildShopContext Current = new();
}
