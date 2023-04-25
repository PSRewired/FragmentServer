using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Articles;
using Fragment.NetSlum.Networking.Packets.Response.Mail;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.Articles;

[FragmentPacket(OpCodes.Data, OpCodes.DataNewCheck)]
public class ArticleCheckRequest : BaseRequest
{
    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        //TODO: Need to get sessions saveID here

        return Task.FromResult<ICollection<FragmentMessage>>(new[] { new ArticleCheckResponse()
            .ArticlesAvailable(true)
            .Build() });
    }
}
