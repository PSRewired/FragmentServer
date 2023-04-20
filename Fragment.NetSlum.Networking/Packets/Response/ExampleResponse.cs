using Fragment.NetSlum.Networking.Constants;

namespace Fragment.NetSlum.Networking.Packets.Response;

public class ExampleResponse : BaseResponse
{
    public override byte[] Build()
    {
        //Example
        return base.Build(OpCodes.Ping, Array.Empty<byte>());
    }
}
