using System;
using System.Buffers;
using System.Collections.Generic;
using Serilog;

namespace Fragment.NetSlum.TcpServer.Buffers;

public class PooledBuffer
{
    private readonly Queue<ArraySegment<byte>> _buffer = new();

    private readonly ArrayPool<byte> _pool = ArrayPool<byte>.Shared;

    public int Length
    {
        get
        {
            int cnt;
            lock (this)
            {
                cnt = _buffer.Count;
            }

            return cnt;
        }
    }

    public void Enqueue(ArraySegment<byte> data)
    {
        lock (this)
        {
            var pooledBuffer = Take(data.Count);
            Buffer.BlockCopy(data.Array!, data.Offset, pooledBuffer, 0, data.Count);

            var segment = new ArraySegment<byte>(pooledBuffer, 0, data.Count);

            _buffer.Enqueue(segment);
        }
    }

    public bool RemoveOne()
    {
        lock (this)
        {
            if (!_buffer.TryDequeue(out var msg))
            {
                return false;
            }

            Return(msg.Array!);
        }

        return true;
    }

    public bool Pop(ref byte[]? payload, out int size)
    {
        lock (this)
        {
            size = 0;

            if (!_buffer.TryDequeue(out var msg))
            {
                return false;
            }

            if (payload == null || payload.Length < msg.Count)
            {
                payload = new byte[msg.Count];
            }

            Buffer.BlockCopy(msg.Array!, msg.Offset, payload, 0, msg.Count);
            size = msg.Count;

            Return(msg.Array!);
        }

        return true;
    }

    public bool Peek(ref byte[]? payload, out int size)
    {
        lock (this)
        {
            size = 0;

            if (!_buffer.TryPeek(out var msg))
            {
                return false;
            }

            if (payload == null || payload.Length < msg.Count)
            {
                payload = new byte[msg.Count];
            }

            Buffer.BlockCopy(msg.Array!, msg.Offset, payload, 0, msg.Count);
            size = msg.Count;
        }

        return true;
    }

    public bool Flush(ref byte[]? payload, out int size)
    {
        lock (this)
        {
            size = 0;

            if (_buffer.Count < 1)
            {
                return false;
            }

            foreach (var message in _buffer)
            {
                size += message.Count;
            }

            if (payload == null || payload.Length < size)
            {
                payload = new byte[size];
            }

            var offset = 0;
            while (_buffer.TryDequeue(out var msg))
            {
                try
                {
                    Buffer.BlockCopy(msg.Array!, msg.Offset, payload, offset, msg.Count);
                    offset += msg.Count;

                    Return(msg.Array!);
                }
                catch (ArgumentException e)
                {
                    Log.Error(e, "Failure while flushing buffer ArrayLen: {Len} - SrcOffset: {SrcOffset} - BufSize: {BufSize} - DstOffset: {DstOffset} - MsgLen: {MsgLen}", msg.Array?.Length, msg.Offset, payload.Length, offset, msg.Count);

                    throw;
                }
            }
        }

        return true;
    }

    public void Clear()
    {
        lock (this)
        {
            while (_buffer.TryDequeue(out var msg))
            {
                try
                {
                    Return(msg.ToArray());
                }
                catch (ArgumentException)
                {
                }
            }
        }
    }

    private byte[] Take(int size)
    {
        return _pool.Rent(size);
    }

    private void Return(byte[] item)
    {
        _pool.Return(item);
    }
}
