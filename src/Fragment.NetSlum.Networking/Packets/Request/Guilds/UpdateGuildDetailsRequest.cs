using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Guilds;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;

namespace Fragment.NetSlum.Networking.Packets.Request.Guilds;

[FragmentPacket(MessageType.Data, OpCodes.DataGuildUpdateDetails)]
public class UpdateGuildDetailsRequest : BaseRequest
{
    private readonly FragmentContext _database;


    public UpdateGuildDetailsRequest(FragmentContext database)
    {
        _database = database;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var reader = new SpanReader(request.Data.Span);

        var guildName = reader.ReadString(out _).ToShiftJisString();
        var guildComment = reader.ReadString(out _).ToShiftJisString();
        var guildEmblem = reader.ReadToByte(0x3d);

        var guild = _database.Guilds.First(g => g.LeaderId == session.CharacterId);

        var logDescription = GetUpdatedInfoDescription(guild, guildComment, guildEmblem);

        guild.Comment = guildComment;
        guild.Emblem = guildEmblem.ToArray();

        _database.Add(new GuildActivityLog
        {
            ActionPerformed = GuildActivityLog.GuildPlayerAction.DetailsUpdated,
            PerformedByCharacterId = session.CharacterId,
            Guild = guild,
            Description = logDescription,
        });

        _database.SaveChanges();

        return SingleMessage(new UpdateGuildDetailsResponse().Build());
    }

    private static string GetUpdatedInfoDescription(Guild guild, string newComment, Span<byte> newEmblem)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Guild information was updated:");

        if (!guild.Comment.Equals(newComment))
        {
            sb.AppendLine($"Name: {guild.Comment} -> {newComment}");
        }

        if (!guild.Emblem.SequenceEqual(newEmblem.ToArray()))
        {
            sb.AppendLine($"Emblem was updated");
        }

        return sb.ToString();
    }
}
