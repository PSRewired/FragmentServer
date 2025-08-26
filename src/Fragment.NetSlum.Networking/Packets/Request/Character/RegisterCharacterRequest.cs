using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Models;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Commands.Characters;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Events;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Character;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.TcpServer.Extensions;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Character;

[FragmentPacket(MessageType.Data, OpCodes.DataRegisterCharRequest)]
public class RegisterCharacterRequest : BaseRequest
{
    private readonly ILogger<RegisterCharacterRequest> _logger;
    private readonly ICommandBus _commandBus;

    public RegisterCharacterRequest(ILogger<RegisterCharacterRequest> logger, ICommandBus commandBus)
    {
        _logger = logger;
        _commandBus = commandBus;
    }

    public override async ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        session.CharacterInfo = CharacterInfo.FromBinaryData(request.Data.Span);
        _logger.LogInformation("Registering character:\n{CharInfo}", session.CharacterInfo.ToString());

        var character = await _commandBus.Execute(new RegisterCharacterCommand(session.PlayerAccountId, session.CharacterInfo));

        GuildStatus guildStatus = GuildStatus.None;

        if(character.Guild != null)
        {
            guildStatus = character.Id == character.Guild.LeaderId ? GuildStatus.GuildMaster : GuildStatus.Member;
            session.GuildId = character.Guild.Id;
        }

        session.CharacterId = character.Id;

        await _commandBus.Notify(new CharacterLoggedInEvent(character.Id, session.Socket!.GetClientIp()));

        return SingleMessageAsync(
            new RegisterCharacterResponse()
                .SetGuildId(character.GuildId ?? 0)
                .SetGuildStatus(guildStatus)
                .Build()
        );
    }
}
