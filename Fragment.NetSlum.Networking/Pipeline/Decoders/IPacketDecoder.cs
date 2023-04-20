using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Pipeline.Decoders;

public interface IPacketDecoder
{
    public int Decode(Memory<byte> data, List<FragmentMessage> messages);
}