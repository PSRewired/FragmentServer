using Fragment.NetSlum.Networking.Constants;

namespace Fragment.NetSlum.Networking.Attributes;

[AttributeUsage(AttributeTargets.Struct|AttributeTargets.Class, AllowMultiple = true)]
public class FragmentPacket : Attribute
{
    public readonly OpCodes OpCode;
    public readonly OpCodes DataPacketType;

    public FragmentPacket(OpCodes opCode)
    {
        OpCode = opCode;
        DataPacketType = OpCodes.None;
    }

    public FragmentPacket(OpCodes opCode, OpCodes dataPacketType)
    {
        OpCode = opCode;
        DataPacketType = dataPacketType;
    }
}
