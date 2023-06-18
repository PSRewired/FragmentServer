using System;
using Fragment.NetSlum.Networking.Constants;

namespace Fragment.NetSlum.Networking.Attributes;

[AttributeUsage(AttributeTargets.Struct|AttributeTargets.Class, AllowMultiple = true)]
public class FragmentPacket : Attribute
{
    public readonly MessageType MessageType;
    public readonly OpCodes DataPacketType;

    public FragmentPacket(MessageType messageType)
    {
        MessageType = messageType;
        DataPacketType = OpCodes.None;
    }

    public FragmentPacket(MessageType messageType, OpCodes dataPacketType)
    {
        MessageType = messageType;
        DataPacketType = dataPacketType;
    }
}
