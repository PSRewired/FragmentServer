using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Crypto;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Pipeline.Encoders;

public class DataTypeEnvelopeEncoder : IMessageEncoder
{
    private uint sequenceNumber = 0xe;
    public void Encode(List<FragmentMessage> responseObjects, MemoryStream memoryStream)
    {
        //sequenceNumber++;
        foreach (var response in responseObjects)
        {
            if (response.OpCode != OpCodes.Data)
            {
                continue;
            }
            var bufferMemory = new Memory<byte>(new byte[GetPaddedBufferLength(response.Data.Length + 8)]);
            var bufferSpan = bufferMemory.Span;

            BinaryPrimitives.WriteUInt32BigEndian(bufferSpan[..4], sequenceNumber++);
            BinaryPrimitives.WriteUInt16BigEndian(bufferSpan[4..6], (ushort) (response.Data.Length + 2));
            BinaryPrimitives.WriteUInt16BigEndian(bufferSpan[6..8], (ushort) response.DataPacketType);
      
            response.Data.CopyTo(bufferMemory[8..]);
            // So we have to generate the checksum at this point and not when we referece response.Data later because it will be wrong
            response.DataChecksum = BlowfishProvider.Checksum(bufferMemory.ToArray()[0..(response.Data.Length + 8)]);

            response.Data = bufferMemory;
        }
    }

    /// <summary>
    /// Fragment requires data packets to be packed into buffers aligned by 8 bytes
    /// </summary>
    /// <param name="dataLength"></param>
    /// <returns></returns>
    private static int GetPaddedBufferLength(int dataLength)
    {
        while ((dataLength + 2 & 7) != 0)
        {
            dataLength++;
        }

        return dataLength;
    }
}
