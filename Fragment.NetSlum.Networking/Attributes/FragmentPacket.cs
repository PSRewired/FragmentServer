using Fragment.NetSlum.Networking.Constants;

namespace Fragment.NetSlum.Networking.Attributes;

[AttributeUsage(AttributeTargets.Struct|AttributeTargets.Class, AllowMultiple = true)]
public class FragmentPacket : Attribute
{
    public OpCodes OpCode;
    public uint PacketClass;
    public uint PacketType;

    public FragmentPacket(OpCodes opCode)
    {
        OpCode = opCode;
    }

    public FragmentPacket(OpCodes opCode, uint packetClass, uint packetType)
    {
        OpCode = opCode;
        PacketClass = packetClass;
        PacketType = packetType;
    }
}
