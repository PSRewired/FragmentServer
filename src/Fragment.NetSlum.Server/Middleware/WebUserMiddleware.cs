using Microsoft.AspNetCore.Http;

namespace Fragment.NetSlum.Server.Middleware;

public class WebUserMiddleware
{
    private readonly RequestDelegate _next;

    public WebUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }
}
