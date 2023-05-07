using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Commands.Accounts;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Saves;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;

namespace Fragment.NetSlum.Networking.Packets.Request.Saves;

[FragmentPacket(OpCodes.Data, OpCodes.DataSaveIdRequest)]
public class GetAccountInfoForSaveIdRequest : BaseRequest
{
    private readonly FragmentContext _database;
    private readonly ICommandBus _commandBus;

    public GetAccountInfoForSaveIdRequest(FragmentContext database, ICommandBus commandBus)
    {
        _database = database;
        _commandBus = commandBus;
    }

    public override async Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var saveId = request.Data.Span.ToShiftJisString();

        var accountId = _database.PlayerAccounts.FirstOrDefault(p => p.SaveId == saveId)?.Id;

        if (accountId == null)
        {
            accountId = await _commandBus.Execute(new RegisterPlayerAccountCommand(saveId));
        }

        session.PlayerAccountId = accountId.Value;

        var serverNews = _database.ServerNews
            .Select(n => n.Content)
            .ToArray();

        var motd = string.Join('\n', serverNews);

        return new[]
        {
            new PlayerAccountInformationResponse()
                .SetAccountId(accountId.Value)
                .SetMessageOfTheDay(motd)
                .Build(),
        };
    }
}
