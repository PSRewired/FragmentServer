using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Api.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Fragment.NetSlum.Server.Api.Endpoints.News;

public class UpdateMotdEndpoint : Endpoint<UpdateMotdRequest, MessageOfTheDay>
{
    private readonly FragmentContext _database;

    public UpdateMotdEndpoint(FragmentContext database)
    {
        _database = database;
    }

    public override void Configure()
    {
        Patch("/motd");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Permissions(nameof(AuthUserPermissions.ManageNews));
        Summary(s =>
        {
            s.Summary = "Updates the current server message of the day";
            s.Description = "Updates the current server message of the day";
        });
    }

    public override async Task<MessageOfTheDay> ExecuteAsync(UpdateMotdRequest req, CancellationToken ct)
    {
        var motd = _database.ServerNews.First();

        motd.Content = req.Content;

        await _database.SaveChangesAsync(ct);

        return new MessageOfTheDay(motd.Content, motd.CreatedAt);
    }
}
