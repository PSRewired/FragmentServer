using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.TcpServer.Options;
using Serilog;

namespace Fragment.NetSlum.TcpServer;

[SuppressMessage("Design", "MA0049:Type name should not match containing namespace")]
public class TcpServer : ITcpServer, IDisposable
{
    private IPEndPoint EndPoint { get; }
    private TcpListener _tcpListener = null!;
    private Task _listenerThread = null!;
    private CancellationTokenSource? _tokenSource;
    private readonly ManualResetEvent _acceptEvent = new(false);

    //private Timer? _connectionTimer;
    private readonly ConcurrentDictionary<Guid, TcpSession> _connectedSessions = new();

    public ArrayPool<byte> BufferPool { get; } = ArrayPool<byte>.Shared;

    /// <summary>
    /// A collection of all currently connected sessions
    /// </summary>
    public TcpSession[] Sessions => GetSessions();

    private ILogger Log => Serilog.Log.ForContext(GetType());

    /// <summary>
    /// Manual mode will cause all sessions to block sending data until their respective Flush() methods are called.
    /// Useful for synchronizing clients in a logic loop, or for packet aggregation
    /// </summary>
    public bool ManualMode { get; }

    // Client socket settings
    /// <summary>
    /// The number of milliseconds to elapse before considering a session timed-out
    /// </summary>
    public int ReceiveTimeoutMs { get; set; } = 1000 * 30;

    /// <summary>
    /// The maximum size of the buffer used to receive data from the socket
    /// </summary>
    public int ReceiveBufferSize { get; set; }

    /// <summary>
    /// The maximum size of the buffer used to send data out to a client
    /// </summary>
    public int SendBufferSize { get; set; }

    public TcpServer(TcpServerOptions options)
    {
        EndPoint = new IPEndPoint(IPAddress.Parse(options.IpAddress), options.Port);
        ManualMode = options.ManualMode;
        ReceiveTimeoutMs = options.SessionTimeout * 1000;
        SendBufferSize = options.SendBufferSize;
        ReceiveBufferSize = options.ReceiveBufferSize;
    }

    protected virtual TcpSession CreateSession()
    {
        return new TcpSession(this);
    }

    public TcpSession? FindSession(Guid id)
    {
        _connectedSessions.TryGetValue(id, out var session);

        return session;
    }

    public void Start()
    {
        if (_tokenSource != null)
        {
            return;
        }

        _tokenSource = new CancellationTokenSource();

        _listenerThread = Task.Factory.StartNew(Listen, _tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
            .Unwrap();
        //_connectionTimer = new Timer(ValidateConnections, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
    }

    public void Stop()
    {
        _tokenSource?.Cancel();
        _acceptEvent.Set();

        DisconnectAllSessions();
        Dispose();
    }

    protected virtual void OnStarted()
    {
        Log.Warning("Listening on {Endpoint}", EndPoint);
    }

    protected virtual void OnSessionConnected(TcpSession session)
    {

    }

    private async Task Listen()
    {
        _tcpListener = new TcpListener(EndPoint);
        _tcpListener.Server.NoDelay = true;
        _tcpListener.Start();

        OnStarted();

        try
        {
            while (!_tokenSource?.IsCancellationRequested ?? false)
            {
                try
                {
                    var socket = await _tcpListener.AcceptSocketAsync(_tokenSource!.Token).ConfigureAwait(false);
                    Log.Verbose("Accepted client {Client}", socket.LocalEndPoint?.ToString());
                    AcceptCallback(socket);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
                catch (InvalidOperationException)
                {
                    break;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
        // Catch socket exception and unexpected errors as they can be informative to the user
        catch (Exception e)
        {
            Log.Error(e, "Unexpected error in listen loop and cannot continue");
        }
        finally
        {
            _tcpListener.Stop();
        }
    }

    private void AcceptCallback(Socket client)
    {
        try
        {
            client.Blocking = true;
            client.NoDelay = true;
            client.ReceiveTimeout = ReceiveTimeoutMs;

            var session = CreateSession()
                .Connect(client);

            _connectedSessions.TryAdd(session.Id, session);
            OnSessionConnected(session);
        }
        catch (ObjectDisposedException) { }
        catch (Exception e)
        {
            Log.Error(e, "Exception while creating TCP session");
        }
    }

    private void DisconnectAllSessions()
    {
        foreach (var session in Sessions)
        {
            session.Disconnect();
        }

        _connectedSessions.Clear();
    }

    public void OnSessionDisconnect(TcpSession session)
    {
        _connectedSessions.Remove(session.Id, out _);
    }

    private TcpSession[] GetSessions()
    {
        return _connectedSessions.Values.ToArray();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _tokenSource?.Dispose();
        _acceptEvent.Dispose();
        //_connectionTimer?.Dispose();
        //_listenerThread.Wait();
        //_listenerThread.Dispose();
    }
}
