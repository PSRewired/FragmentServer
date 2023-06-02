using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(OpCodes.Data, OpCodes.DataGuildGetMenu)]
public class GetGuildMenuRequest : BaseRequest
{
    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        ushort menuId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        if (menuId < 1)
        {
            return Task.FromResult(HandleMenuCategories());
        }

        return Task.FromResult<ICollection<FragmentMessage>>(new[]
        {
            new GuildListEntryCountResponse(1).Build(),
            new GuildMenuListEntryResponse()
                .SetGuildId(1)
                .SetGuildName("Test Guild")
                .Build()
        });
    }

    private ICollection<FragmentMessage> HandleMenuCategories()
    {
        return new[]
        {
            new GuildMenuCategoryCountResponse(1).Build(),
            new GuildMenuCategoryResponse()
                .SetCategoryId(1)
                .SetCategoryName("ALL")
                .Build()
        };
    }
}
