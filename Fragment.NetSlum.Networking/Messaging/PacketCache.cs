using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Messaging;

public class PacketCache
{
    private readonly Dictionary<OpCodes, Dictionary<uint, Dictionary<uint, Type>>> _packetReferences = new();

    public void AddRequest(FragmentPacket msg, Type t)
    {
        EnsureCreated(msg);

        var existingType = _packetReferences[msg.OpCode][msg.PacketClass][msg.PacketType];
        if (existingType != null)
        {
            throw new InvalidConstraintException(
                $"Attempted to add packet {t.Name} when existing reference already exists. ({existingType.Name})");
        }

        _packetReferences[msg.OpCode][msg.PacketClass][msg.PacketType] = t;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization|MethodImplOptions.AggressiveInlining)]
    public Type? GetRequest(FragmentMessage o)
    {
        uint packetClass = 0;
        uint packetType = 0;

        var rtType = o.OpCode;

        if (!_packetReferences.ContainsKey(rtType))
        {
            return null;
        }

        if (!_packetReferences[rtType].ContainsKey(packetClass))
        {
            return null;
        }

        if (!_packetReferences[rtType][packetClass].ContainsKey(packetType))
        {
            return null;
        }

        return _packetReferences[rtType][packetClass][packetType];
    }

    private void EnsureCreated(FragmentPacket msg)
    {
        if (!_packetReferences.ContainsKey(msg.OpCode))
        {
            _packetReferences[msg.OpCode] = new Dictionary<uint, Dictionary<uint, Type>>();
        }

        if (!_packetReferences[msg.OpCode].ContainsKey(msg.PacketClass))
        {
            _packetReferences[msg.OpCode][msg.PacketClass] = new Dictionary<uint, Type>();
        }

        if (!_packetReferences[msg.OpCode][msg.PacketClass].ContainsKey(msg.PacketType))
        {
            _packetReferences[msg.OpCode][msg.PacketClass][msg.PacketType] = null;
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder("=== Registered Packets ===");
        sb.AppendLine();

        foreach (var (type, classes) in _packetReferences)
        {
            sb.AppendLine($"{type} ({((byte) type):X2})");
            foreach (var (cls, pTypes) in classes)
            {
                foreach (var (pType, rType) in pTypes)
                {
                    sb.AppendLine($"    Type: {pType:X2} -> {rType}");
                }
            }
        }

        return sb.ToString();
    }
}
