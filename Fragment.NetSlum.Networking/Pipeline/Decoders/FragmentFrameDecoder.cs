using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Pipeline.Decoders;

public class FragmentFrameDecoder : IPacketDecoder
{
    public int Decode(Memory<byte> data, List<FragmentMessage> messages)
    {
        //TODO
        throw new NotImplementedException();
    }
}
