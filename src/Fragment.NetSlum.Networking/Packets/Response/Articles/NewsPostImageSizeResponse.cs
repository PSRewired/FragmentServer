using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Articles;

public class NewsPostImageSizeResponse : BaseResponse
{
    private readonly uint _imageSize;
    private readonly ushort _chunkSize;

    public NewsPostImageSizeResponse(uint imageSize, ushort chunkSize)
    {
        _imageSize = imageSize;
        _chunkSize = chunkSize;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[sizeof(uint) + sizeof(ushort)]);
        var bufferSpan = buffer.Span;

        BinaryPrimitives.WriteUInt32BigEndian(bufferSpan, _imageSize);
        BinaryPrimitives.WriteUInt16BigEndian(bufferSpan[4..], _chunkSize);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataNewsPostSizeInfoResponse,
            Data = buffer
        };
    }
}
