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

[FragmentPacket(OpCodes.Data, OpCodes.DataGuildCreate)]
public class CreateGuildRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public CreateGuildRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var reader = new SpanReader(request.Data.Span);

        var guildName = reader.ReadString(out _).ToShiftJisString();
        var guildComment = reader.ReadString(out _).ToShiftJisString();
        var guildEmblem = reader.ReadToByte(0x3d);

        var myCharacter = _database.Characters.First(c => c.Id == session.CharacterId);

        var guild = new Guild
        {
            Name = guildName,
            Comment = guildComment,
            Emblem = guildEmblem.ToArray(),
            Leader = myCharacter,
            Members = { myCharacter },
        };

        _database.Add(guild);
        _database.SaveChanges();

        return Task.FromResult<ICollection<FragmentMessage>>(new[]
        {
            new CreateGuildResponse(guild.Id).Build(),
        });
    }
}
