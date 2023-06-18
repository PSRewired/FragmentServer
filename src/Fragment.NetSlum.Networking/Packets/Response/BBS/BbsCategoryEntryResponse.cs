using System.Runtime.InteropServices;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.BBS;

public class BbsCategoryEntryResponse : BaseResponse
{
    private ushort _categoryId;
    private string _categoryTitle = "";

    public BbsCategoryEntryResponse SetCategoryId(ushort id)
    {
        _categoryId = id;

        return this;
    }

    public BbsCategoryEntryResponse SetCategoryTitle(string title)
    {
        _categoryTitle = title;

        return this;
    }

    public override FragmentMessage Build()
    {
        var titleBytes = _categoryTitle.ToShiftJis();

        var writer = new MemoryWriter(titleBytes.Length + Marshal.SizeOf(_categoryId));
        writer.Write(_categoryId);
        writer.Write(titleBytes);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataBbsEntryCategory,
            Data = writer.Buffer,
        };
    }
}
