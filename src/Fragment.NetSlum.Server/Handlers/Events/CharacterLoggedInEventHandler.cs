using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus.Contracts.Events;
using Fragment.NetSlum.Networking.Events;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;

namespace Fragment.NetSlum.Server.Handlers.Events;

public class CharacterLoggedInEventHandler : EventHandler<CharacterLoggedInEvent>
{
    private readonly FragmentContext _database;

    public CharacterLoggedInEventHandler(FragmentContext database)
    {
        _database = database;
    }

    public override async ValueTask Handle(CharacterLoggedInEvent eventInfo, CancellationToken cancellationToken)
    {
        _database.CharacterIpLogs.Add(new CharacterIpLog
        {
            CharacterId = eventInfo.CharacterId,
            IpAddress = eventInfo.IpAddress
        });

        await _database.SaveChangesAsync(cancellationToken);
    }
}
