using System.Collections.Generic;
using Fragment.NetSlum.Core.CommandBus.Contracts.Queries;

namespace Fragment.NetSlum.Networking.Queries.Authentication;

public record GetAuthUserPermissionsQuery(string Username) : IQuery<IEnumerable<string>>;
