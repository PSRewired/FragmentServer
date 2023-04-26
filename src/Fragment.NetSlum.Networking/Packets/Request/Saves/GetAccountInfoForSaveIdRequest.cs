using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Saves;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.Saves;

[FragmentPacket(OpCodes.Data, OpCodes.DataSaveId)]
public class GetAccountInfoForSaveIdRequest : BaseRequest
{
    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var saveId = request.Data.Span.ToShiftJisString();

        //TODO: Create/fetch accountID information
        var accountId = 5;

        var motd = @"Welcome to Netslum-Redux!
Current Status:
- Lobby #GOnline#W!
- BBS #GOnline#W!
- Mail #GOnline#W!
- Guilds #GOnline#W!
- Ranking #GOnline#W!
- News #GOnline#W!";

        return Task.FromResult<ICollection<FragmentMessage>>(new[]
        {
            new PlayerAccountInformationResponse()
                .SetAccountId(accountId)
                .SetMessageOfTheDay(motd)
                .Build(),
        });
    }
}
