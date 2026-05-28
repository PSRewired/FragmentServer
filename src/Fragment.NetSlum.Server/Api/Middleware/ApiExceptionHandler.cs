using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Fragment.NetSlum.Server.Api.Middleware;

public class ApiExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public ApiExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case KeyNotFoundException keyNotFoundException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails =
                    {
                        Detail = exception.Message,
                    },
                    Exception = keyNotFoundException,
                });
                return true;
        }

        return false;
    }
}
