using System.Buffers.Binary;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Articles;

public class NewsCategoryEntryResponse : BaseResponse
{
    private ushort categoryId { get; set; }
    private string categoryName { get; set; } = "";

    public NewsCategoryEntryResponse SetCategoryId(ushort id)
    {
        categoryId = id;

        return this;
    }

    public NewsCategoryEntryResponse SetCategoryName(string name)
    {
        categoryName = name;

        return this;
    }

    public override FragmentMessage Build()
    {
        var nameBytes = categoryName.ToShiftJis();

        // Need to reserve 1 extra byte to act as a null terminator for the category name
        var buffer = new Memory<byte>(new byte[nameBytes.Length + sizeof(ushort) + 1]);
        var bufferSpan = buffer.Span;

        BinaryPrimitives.WriteUInt16BigEndian(bufferSpan, categoryId);
        nameBytes.CopyTo(bufferSpan[2..]);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataNewsEntryCategory,
            Data = buffer,
        };
    }
}
