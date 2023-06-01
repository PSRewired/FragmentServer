using System.Buffers.Binary;
using Fragment.NetSlum.Core.Extensions;

namespace Fragment.NetSlum.Core.Buffers;

public ref struct SpanReader
{
    private Span<byte> _holdingSpan;
    private int _position;

    public SpanReader(Span<byte> buffer)
    {
        _holdingSpan = buffer;
    }

    /// <summary>
    /// Reads the next available unsigned 16-bit integer from the buffer
    /// </summary>
    /// <returns></returns>
    public ushort ReadUInt16()
    {
        const int size = sizeof(ushort);

        try
        {
            return BinaryPrimitives.ReadUInt16BigEndian(_holdingSpan[_position..(_position + size)]);
        }
        finally
        {
            _position += size;
        }
    }

    /// <summary>
    /// Reads the next available unsigned 32-bit integer from the buffer
    /// </summary>
    /// <returns></returns>
    public uint ReadUInt32()
    {
        const int size = sizeof(uint);

        try
        {
            return BinaryPrimitives.ReadUInt32BigEndian(_holdingSpan[_position..(_position + size)]);
        }
        finally
        {
            _position += size;
        }
    }

    /// <summary>
    /// Reads the next available 16-bit integer from the buffer
    /// </summary>
    /// <returns></returns>
    public short ReadInt16()
    {
        const int size = sizeof(short);

        try
        {
            return BinaryPrimitives.ReadInt16BigEndian(_holdingSpan[_position..(_position + size)]);
        }
        finally
        {
            _position += size;
        }
    }

    /// <summary>
    /// Reads the next available 32-bit integer from the buffer
    /// </summary>
    /// <returns></returns>
    public int ReadInt32()
    {
        const int size = sizeof(int);

        try
        {
            return BinaryPrimitives.ReadInt16BigEndian(_holdingSpan[_position..(_position + size)]);
        }
        finally
        {
            _position += size;
        }
    }

    /// <summary>
    /// Reads an arbitrary number of bytes from the buffer
    /// </summary>
    /// <param name="numBytes"></param>
    /// <returns></returns>
    public Span<byte> Read(int numBytes)
    {
        try
        {
            return _holdingSpan[_position..(_position + numBytes)];
        }
        finally
        {
            _position += numBytes;
        }
    }

    /// <summary>
    /// Reads a single byte from the buffer
    /// </summary>
    /// <returns></returns>
    public byte ReadByte()
    {
        try
        {
            return _holdingSpan[_position];
        }
        finally
        {
            _position += 1;
        }
    }

    /// <summary>
    /// Returns the byte representation of a string until a null-terminator is encountered
    /// </summary>
    /// <param name="numBytes"></param>
    /// <returns></returns>
    public Span<byte> ReadString(out int numBytes)
    {
        var bytes = _holdingSpan[_position..].ReadToNullByte();
        numBytes = bytes.Length;
        _position += numBytes;

        return bytes;
    }

    /// <summary>
    /// Offsets the current position by the given number of bytes
    /// </summary>
    /// <param name="numBytes"></param>
    public void Skip(int numBytes)
    {
        _position += numBytes;
    }
}
