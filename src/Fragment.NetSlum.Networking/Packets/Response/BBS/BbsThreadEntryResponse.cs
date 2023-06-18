using System.Runtime.InteropServices;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.BBS;

public class BbsThreadEntryResponse : BaseResponse
{
    private int _threadId;
    private string _threadTitle = "";

    public BbsThreadEntryResponse SetThreadId(int id)
    {
        _threadId = id;

        return this;
    }

    public BbsThreadEntryResponse SetThreadTitle(string title)
    {
        _threadTitle = title;

        return this;
    }

    public override FragmentMessage Build()
    {
        var titleBytes = _threadTitle.ToShiftJis();

        var writer = new MemoryWriter(titleBytes.Length + Marshal.SizeOf(_threadId));
        writer.Write(_threadId);
        writer.Write(titleBytes);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataBbsEntryThread,
            Data = writer.Buffer,
        };
    }
}
