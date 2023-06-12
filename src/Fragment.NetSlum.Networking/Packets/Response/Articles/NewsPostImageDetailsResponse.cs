using System;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Articles;

public class NewsPostImageDetailsResponse : BaseResponse
{
    private readonly Memory<byte> _colorData;
    private readonly Memory<byte> _imageData;

    public NewsPostImageDetailsResponse(Memory<byte> colorData, Memory<byte> imageData)
    {
        _colorData = colorData;
        _imageData = imageData;
    }

    public override FragmentMessage Build()
    {
        //var buffer = new Memory<byte>(new byte[sizeof(uint) + _imageData.Length + _colorData.Length]);
        //var bufferSpan = buffer.Span;

        //var pos = 0;
        //BinaryPrimitives.WriteUInt32BigEndian(bufferSpan[pos..], (ushort)(_imageData.Length + _colorData.Length));

        //pos = sizeof(uint);
        //_colorData.CopyTo(buffer[pos..]);
        //pos += _colorData.Length;

        //_imageData.CopyTo(buffer[pos..]);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataNewsPostDetailsResponse,
            Data = _imageData,
        };
    }
}
