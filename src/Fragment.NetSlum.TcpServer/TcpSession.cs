using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.TcpServer.Buffers;
using Serilog;

namespace Fragment.NetSlum.TcpServer;

public class TcpSession : IDisposable
{
    /// <summary>
    /// Unique identifier for this session
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// TCP server instance this session is connected to
    /// </summary>
    public ITcpServer Server { get; }

    /// <summary>
    /// The socket reference for this session
    /// </summary>
    public Socket? Socket { get; private set; }

    /// <summary>
    /// Indicates whether this session requires manual Flush() calls to send data
    /// </summary>
    public bool RequiresManualSends => Server.ManualMode;

    /// <summary>
    /// The connection status of this session
    /// </summary>
    public bool IsConnected => Socket != null && (Socket.Connected && !IsDisconnecting);

    /// <summary>
    /// The amount of elapsed time since the last time this session received data from the connected client
    /// </summary>
    public long LastReceivedMs => _lastReceiveTimer.ElapsedMilliseconds;

    private bool IsDisconnecting { get; set; }

    private readonly PooledBuffer _sendQueue = new();
    private readonly ManualResetEvent _sendQueueLock = new(false);

    private readonly byte[] _receiveBuffer;

    private bool _isDisposed;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly Stopwatch _lastReceiveTimer;
    private readonly List<Task> _tasks = new();

    public TcpSession(ITcpServer server)
    {
        Server = server;
        //_receiveBuffer = GC.AllocateArray<byte>(32768, pinned: true);
        _receiveBuffer = server.BufferPool.Rent(16384);
        _cancellationTokenSource = new CancellationTokenSource();
        _lastReceiveTimer = Stopwatch.StartNew();
    }

    protected internal TcpSession Connect(Socket socket)
    {
        Socket = socket;

        Socket.ReceiveBufferSize = _receiveBuffer.Length;
        Socket.SendBufferSize = _receiveBuffer.Length;

        OnConnected();

        _tasks.Add(Task.Factory.StartNew(DoReceive, TaskCreationOptions.LongRunning).Unwrap());
        _tasks.Add(Task.Factory.StartNew(SendLoop, TaskCreationOptions.LongRunning));

        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private async Task DoReceive()
    {
        do
        {
            try
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                Socket!.SendBufferSize = Server.SendBufferSize;
                Socket!.ReceiveBufferSize = Server.ReceiveBufferSize;
                Socket!.ReceiveTimeout = Server.ReceiveTimeoutMs;

                // This synchronous call will block until data is received or the configured ReceiveTimeout has elapsed.
                var read = Socket!.Receive(_receiveBuffer, SocketFlags.None);

                // If zero bytes were received, this indicates that the socket has timed out or disconnected
                if (read < 1)
                {
                    break;
                }

                _lastReceiveTimer.Restart();

                // In order to not have to chain cancellation tokens throughout the OnReceived method, we need to spawn a new
                // task and make it cancellable via our own internal cancellation token
                await OnReceived(_receiveBuffer.AsMemory()[..read], _cancellationTokenSource.Token);
            }
            catch (ObjectDisposedException)
            {
                // If the Socket has been disposed, quit.
                break;
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (SocketException socketException)
            {
                // Catch the WSAEWOULDBLOCK error here so the thread tries to re-fetch the data
                if (socketException.SocketErrorCode == SocketError.WouldBlock)
                {
                    continue;
                }

                OnError(socketException.SocketErrorCode);
                break;
            }
            catch (Exception e)
            {
                Log.Error(e, "DoReceive for session {Id} caught an error", Id);
                break;
            }
        } while (IsConnected);

        Log.Information("Session {SessionId} receive thread stopped!", Id);
        Disconnect();
    }


    public virtual void Disconnect()
    {
        if (IsDisconnecting)
        {
            return;
        }

        IsDisconnecting = true;

        if (_cancellationTokenSource.IsCancellationRequested || _isDisposed)
        {
            return;
        }

        Log.Verbose("Disconnecting socket");
        Socket?.Shutdown(SocketShutdown.Both);
        Log.Verbose("Invoking server session disconnect method");
        Server.OnSessionDisconnect(this);
        Log.Verbose("Closing socket");
        Socket?.Close();
        Server.BufferPool.Return(_receiveBuffer);
        Log.Verbose("Releasing send buffer");
        _sendQueue.Clear();
        Log.Verbose("Setting signal for threads to stop");
        _cancellationTokenSource.Cancel();
        Log.Verbose("Releasing semaphores to ensure all threads are not being blocked");
        _sendQueueLock.Set();
        Log.Verbose("Invoking session OnDisconnected callback");
        OnDisconnected();


        Log.Verbose("Disposing of session resources");
        Dispose();
        Log.Verbose("Disconnect complete!");
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        Log.Debug("Disposing TCP session {Id}", Id);

        _isDisposed = true;

        if (!disposing)
        {
            return;
        }

        if (IsConnected)
        {
            Disconnect();
            return;
        }

        Log.Verbose("Releasing thread locks ");
        _sendQueueLock.Set();
        Log.Verbose("Disposing cancellation token");
        _cancellationTokenSource.Dispose();
        Log.Debug("TCP session {Id} dispose complete!", Id);
        _sendQueueLock.Dispose();
    }

    // Helper function for backwards-compat of old TcpSession
    public void SendAsync(Span<byte> data)
    {
        Send(data);
    }

    public void Send(ReadOnlySpan<byte> data)
    {
        // If the buffer is empty or the send loop has stopped, do nothing
        if (data.Length < 1 || _sendQueueLock.SafeWaitHandle.IsClosed)
        {
            return;
        }

        _sendQueue.Enqueue(data.ToArray());

        // Use server's manual mode option, or default to interrupting the send thread.
        if (!RequiresManualSends)
        {
            _sendQueueLock.Set();
        }
    }

    public virtual void Flush()
    {
        // If the server is not in manual mode, the socket is disconnected, or the buffer is empty dont do anything.
        if (!RequiresManualSends || _sendQueue.Length < 1 || !IsConnected)
        {
            return;
        }

        if (_sendQueueLock.SafeWaitHandle.IsClosed)
        {
            return;
        }

#if (DEBUG)
        Log.Verbose("Flushing session with {Num} messages", _sendQueue.Length);
#endif
        _sendQueueLock.Set();
    }

    protected virtual Task OnReceived(Memory<byte> data, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected virtual void OnConnected()
    {
    }

    protected virtual void OnDisconnected()
    {
    }

    protected virtual void OnError(SocketError error)
    {
        Log.Warning("[{Cls}] socket encountered an error {Error}", GetType().Name, error);
    }

    private void SendLoop()
    {
        byte[]? messageBuf = null;

        try
        {
            while (!_cancellationTokenSource.IsCancellationRequested && IsConnected)
            {
                try
                {
                    _sendQueueLock.Reset();

                    if (Socket == null)
                    {
                        break;
                    }

                    if (_sendQueue.Flush(ref messageBuf, out var messageSize))
                    {
                        var sent = Socket.Send(messageBuf.AsSpan()[..messageSize], SocketFlags.None, out var error);

                        // If the socket instance is terminating, this thread is no longer needed
                        if (error is SocketError.Shutdown or SocketError.ConnectionReset or SocketError.ConnectionAborted or SocketError.OperationAborted)
                        {
                            Log.Information("TcpSession {TcpSessionId} send loop is stopped due to failed connection ({SocketError})", Id, error);
                            break;
                        }

                        if (sent < 1 && error != SocketError.Success)
                        {
                            Log.Error("Data send failure on session {Error} -- {Len}", error, messageSize);
                            break;
                        }

                        Log.Verbose("Send thread for session {Id} sent {Count} bytes of data", Id, messageSize);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e, "Session send loop has encountered an error");
                    break;
                }

                _sendQueueLock.WaitOne();
            }

            Log.Debug("Session {Id} send loop has stopped!", Id);
        }
        catch (Exception e)
        {
            Log.Error(e, "Critical send loop error for session {Id}", Id);
        }
    }
}
