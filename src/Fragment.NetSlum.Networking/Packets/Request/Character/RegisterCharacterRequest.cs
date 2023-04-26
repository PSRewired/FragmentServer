using Fragment.NetSlum.Core.Models;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Character;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Character;

[FragmentPacket(OpCodes.Data, OpCodes.DataRegisterChar)]
public class RegisterCharacterRequest : BaseRequest
{
    private readonly ILogger<RegisterCharacterRequest> _logger;

    public RegisterCharacterRequest(ILogger<RegisterCharacterRequest> logger)
    {
        _logger = logger;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        session.CharacterInfo = CharacterInfo.FromBinaryData(request.Data.Span);
        _logger.LogInformation("Registering character:\n{CharInfo}", session.CharacterInfo.ToString());

        return Task.FromResult<ICollection<FragmentMessage>>(new[]
        {
            new RegisterCharacterResponse()
                .Build()
        });
    }
}
