using System.Buffers.Binary;

namespace Fragment.NetSlum.Core.Buffers;

public class MemoryWriter
{
    private int _position;
    public readonly Memory<byte> Buffer;

    private int BufferLength => Buffer.Length;

    public MemoryWriter(int size)
    {
        Buffer = new Memory<byte>(new byte[size]);
    }

    public MemoryWriter(Memory<byte> existingBuffer, int startingOffset = 0)
    {
        Buffer = existingBuffer;
        _position = startingOffset;
    }

    public MemoryWriter SetPosition(int pos)
    {
        _position = pos;

        return this;
    }

    public MemoryWriter Skip(int bytes)
    {
        _position += bytes;

        return this;
    }

    /// <summary>
    /// Writes a numerical type in big-endian format to the buffer
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="TData"></typeparam>
    /// <returns></returns>
    public MemoryWriter Write<TData>(TData value) where TData : struct
    {
        var size = 0;
        switch (value)
        {
            case int vVal:
                size = sizeof(int);
                BinaryPrimitives.WriteInt32BigEndian(Buffer.Span[_position..(_position + size)], vVal);
                break;
            case uint vVal:
                size = sizeof(uint);
                BinaryPrimitives.WriteUInt32BigEndian(Buffer.Span[_position..(_position + size)], vVal);
                break;
            case short vVal:
                size = sizeof(short);
                BinaryPrimitives.WriteInt16BigEndian(Buffer.Span[_position..(_position + size)], vVal);
                break;
            case ushort vVal:
                size = sizeof(ushort);
                BinaryPrimitives.WriteUInt16BigEndian(Buffer.Span[_position..(_position + size)], vVal);
                break;
            case long vVal:
                size = sizeof(long);
                BinaryPrimitives.WriteInt64BigEndian(Buffer.Span[_position..(_position + size)], vVal);
                break;
            case ulong vVal:
                size = sizeof(ulong);
                BinaryPrimitives.WriteUInt64BigEndian(Buffer.Span[_position..(_position + size)], vVal);
                break;
            case byte vVal:
                size = sizeof(byte);
                Buffer.Span[_position] = vVal;
                break;
        }

        _position += size;

        return this;
    }

    /// <summary>
    /// Writes an arbitrary dataset to the buffer
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public MemoryWriter Write(Memory<byte> data)
    {
        var newPos = _position + data.Length;

        if (newPos > BufferLength)
        {
            throw new ArgumentOutOfRangeException(
                $"Attempted to write {data.Length} bytes to a buffer that only has {Buffer.Length - _position} remaining");
        }

        data.CopyTo(Buffer[_position..(_position + data.Length)]);
        _position = newPos;

        return this;
    }

    public MemoryWriter Write(Span<byte> data)
    {
        var newPos = _position + data.Length;

        if (newPos > BufferLength)
        {
            throw new ArgumentOutOfRangeException(
                $"Attempted to write {data.Length} bytes to a buffer that only has {Buffer.Length - _position} remaining");
        }

        data.CopyTo(Buffer[_position..(_position + data.Length)].Span);
        _position = newPos;

        return this;
    }
}
