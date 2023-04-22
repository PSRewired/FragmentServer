using System;
using System.Buffers;

namespace Fragment.NetSlum.TcpServer;

public interface ITcpServer
{
    public bool ManualMode { get; }
    protected internal ArrayPool<byte> BufferPool { get; }
    public TcpSession[] Sessions { get; }

    public TcpSession? FindSession(Guid id);
    public void OnSessionDisconnect(TcpSession session);
    public void Start();
    public void Stop();
}
