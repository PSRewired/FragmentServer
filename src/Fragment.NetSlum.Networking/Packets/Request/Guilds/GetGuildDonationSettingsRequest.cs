using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataGuildGetDonationSettings)]
public class GetGuildDonationSettingsRequest : BaseRequest
{
    private readonly FragmentContext _database;
    private readonly ILogger<GetGuildDonationSettingsRequest> _logger;

    public GetGuildDonationSettingsRequest(FragmentContext database, ILogger<GetGuildDonationSettingsRequest> logger)
    {
        _database = database;
        _logger = logger;
    }
    public override async ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var guild = await _database.Guilds
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == session.GuildId);

        var isGuildMaster = guild != null && guild.LeaderId == session.GuildId;

        _logger.LogInformation("Guild donation settings requested for guild ID {GuildId} by player {PlayerId}({PlayerName}). Is Guild Master? {IsGuildMaster}", session.GuildId, session.CharacterId, session.CharacterInfo!.CharacterName, isGuildMaster);

        return SingleMessageAsync(new GuildDonationSettingsResponse()
            .SetIsGuildMaster(isGuildMaster)
            .Build());
    }
}
