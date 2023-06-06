using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Core.Models;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Commands.Characters;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Character;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Character;

[FragmentPacket(OpCodes.Data, OpCodes.DataRegisterCharRequest)]
public class RegisterCharacterRequest : BaseRequest
{
    private readonly ILogger<RegisterCharacterRequest> _logger;
    private readonly ICommandBus _commandBus;

    public RegisterCharacterRequest(ILogger<RegisterCharacterRequest> logger, ICommandBus commandBus)
    {
        _logger = logger;
        _commandBus = commandBus;
    }

    public override async Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        session.CharacterInfo = CharacterInfo.FromBinaryData(request.Data.Span);
        _logger.LogInformation("Registering character:\n{CharInfo}", session.CharacterInfo.ToString());

        var character = await _commandBus.Execute(new RegisterCharacterCommand(session.CharacterInfo));

        session.CharacterId = character.Id;

        return new[]
        {
            new RegisterCharacterResponse()
                .Build()
        };
    }
}
