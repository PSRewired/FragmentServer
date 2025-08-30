using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataGuildShopItemCatalogRequest)]
public class GetGuildShopItemCatalogRequest : BaseRequest
{
    private readonly FragmentContext _database;
    private readonly ILogger<GetGuildShopItemCatalogRequest> _logger;

    public GetGuildShopItemCatalogRequest(FragmentContext database, ILogger<GetGuildShopItemCatalogRequest> logger)
    {
        _database = database;
        _logger = logger;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var reader = new SpanReader(request.Data.Span);
        var categoryId = reader.ReadUInt16();

        // The old database classified items as the category ID + item ID. So we need to make this backwards compatible with that...
        reader.Skip(-2);
        var itemId = reader.ReadUInt32();

        _logger.LogDebug("GetGuildShopItemCatalogRequest: categoryId={CategoryId}, itemId={ItemId}", categoryId, itemId);

        var items = _database.GuildShopItems
            .AsNoTracking()
            .Include(i => i.Guild)
            .Where(i => i.ItemId == itemId);

        var itemCount = items.Count();

        var responses = new List<FragmentMessage>();

        responses.Add(new GuildShopItemCatalogCountResponse((ushort)itemCount).Build());

        foreach (var item in items)
        {
            responses.Add(new GuildShopItemCatalogEntryResponse()
                .SetShopId(item.GuildId)
                .SetItemId((uint)item.ItemId)
                .SetShopName(item.Guild.Name)
                .SetAmount(item.Price)
                .SetQuantity(item.Quantity)
            .Build());
        }

        return ValueTask.FromResult<ICollection<FragmentMessage>>(responses);
    }
}
