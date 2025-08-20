using System;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Server.Servers;
using Fragment.NetSlum.TcpServer;
using Haukcode.HighResolutionTimer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fragment.NetSlum.Server.Services;

public class ClientTickService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private Task _clientTickTask = null!;

    public ClientTickService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _clientTickTask = Task.Factory.StartNew(() => FlushClients(cancellationToken), TaskCreationOptions.LongRunning);

        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        await _clientTickTask.WaitAsync(CancellationToken.None);
    }

    private void FlushClients(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ClientTickService>>();
        var server = scope.ServiceProvider.GetRequiredService<ITcpServer>();
        var serverOptions = scope.ServiceProvider.GetRequiredService<IOptions<ServerConfiguration>>();

        var timer = new HighResolutionTimer();

        try
        {
            var tickRateMs = 1000 / serverOptions.Value.TickRate;
            logger.LogWarning("Client tick service is active with tick rate of {TickRate}/s", serverOptions.Value.TickRate);

            // Evaluate all sessions every 5 milliseconds
            timer.SetPeriod(5);
            timer.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var client in server.Sessions)
                {
                    if (client is not FragmentTcpSession tcpSession)
                    {
                        continue;
                    }

                    // If the client has not exceeded the tick rate window, skip them
                    if (tcpSession.ElapsedMillisecondsSinceLastFlush < tickRateMs)
                    {
                        continue;
                    }

                    try
                    {
                        tcpSession.Flush();
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Failed to flush data for session {Id}", tcpSession.Id);
                    }
                }

                timer.WaitForTrigger();
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Client tick service encountered a fatal error");
        }
        finally
        {
            timer.Stop();
            timer.Dispose();
            logger.LogWarning("Client tick service has stopped!");
        }
    }
}
