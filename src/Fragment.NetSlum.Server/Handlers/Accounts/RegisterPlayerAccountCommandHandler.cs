using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus.Contracts.Commands;
using Fragment.NetSlum.Networking.Commands.Accounts;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;

namespace Fragment.NetSlum.Server.Handlers.Accounts;

public class RegisterPlayerAccountCommandHandler : CommandHandler<RegisterPlayerAccountCommand, int>
{
    private readonly FragmentContext _database;

    public RegisterPlayerAccountCommandHandler(FragmentContext database)
    {
        _database = database;
    }

    public override async ValueTask<int> Handle(RegisterPlayerAccountCommand command, CancellationToken cancellationToken)
    {
        var newAccount = new PlayerAccount
        {
            SaveId = command.SaveId,
        };

        _database.PlayerAccounts.Add(newAccount);

        await _database.SaveChangesAsync(cancellationToken);

        return newAccount.Id;
    }
}
