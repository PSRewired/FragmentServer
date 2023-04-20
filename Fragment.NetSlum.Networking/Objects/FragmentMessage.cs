using Fragment.NetSlum.Networking.Constants;

namespace Fragment.NetSlum.Networking.Objects;

public class FragmentMessage
{
    public OpCodes OpCode { get; set; }
    public Memory<byte> Data { get; set; }

    public byte[] ToArray()
    {
        //TODO
        return Array.Empty<byte>();
    }
}
