using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Pipeline.Encoders;

public class DataTypeEnvelopeEncoder : IMessageEncoder
{
    private uint sequenceNumber = 0xe;
    public void Encode(List<FragmentMessage> responseObjects, MemoryStream memoryStream)
    {
        foreach (var response in responseObjects)
        {
            if (response.MessageType != MessageType.Data)
            {
                continue;
            }
            var bufferMemory = new Memory<byte>(new byte[response.Data.Length + 8]);
            var bufferSpan = bufferMemory.Span;

            BinaryPrimitives.WriteUInt32BigEndian(bufferSpan[..4], sequenceNumber++);
            BinaryPrimitives.WriteUInt16BigEndian(bufferSpan[4..6], (ushort) (response.Data.Length + 2));
            BinaryPrimitives.WriteUInt16BigEndian(bufferSpan[6..8], (ushort) response.DataPacketType);
            response.Data.CopyTo(bufferMemory[8..]);
            response.Data = bufferMemory;
        }
    }
}
