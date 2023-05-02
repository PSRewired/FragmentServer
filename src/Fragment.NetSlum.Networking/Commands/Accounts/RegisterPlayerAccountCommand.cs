using Fragment.NetSlum.Core.CommandBus.Contracts.Commands;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Commands.Accounts;

/// <summary>
/// Registers a new player account. Since player Save IDs must be unique, you will need to check for an existing account yourself,
/// otherwise this command may throw an exception
/// <exception cref="DbUpdateException"></exception>
/// <returns>The account ID of the new player</returns>
/// </summary>
public class RegisterPlayerAccountCommand : ICommand<int>
{
    public string SaveId { get; set; }

    public RegisterPlayerAccountCommand(string saveId)
    {
        SaveId = saveId;
    }
}
