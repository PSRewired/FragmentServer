using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Constants;

namespace Fragment.NetSlum.Networking.Packets.Response.Articles;

public class NewsArticleEntryResponse : BaseResponse
{
    public ushort articleId { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime DateCreated { get; set; } = DateTime.MinValue;
    public bool Read { get; set; }

    public NewsArticleEntryResponse SetArticleId(ushort id)
    {
        articleId = id;

        return this;
    }

    public NewsArticleEntryResponse SetTitle(string title)
    {
        Title = title;

        return this;
    }

    public NewsArticleEntryResponse SetContent(string content)
    {
        Content = content;

        return this;
    }

    public NewsArticleEntryResponse SetArticleCreationDate(DateTime createdAt)
    {
        DateCreated = createdAt;

        return this;
    }

    public NewsArticleEntryResponse IsRead(bool read)
    {
        Read = read;

        return this;
    }

    public override FragmentMessage Build()
    {
        var titleBytes = Title.PadRight(0x22, char.MinValue).ToShiftJis();
        var contentBytes = Content.PadRight(0x25a, char.MinValue).ToShiftJis();

        var buffer = new Memory<byte>(new byte[titleBytes.Length + contentBytes.Length + sizeof(ushort) + sizeof(ulong) + sizeof(uint)]);
        var bufferSpan = buffer.Span;

        BinaryPrimitives.WriteUInt16BigEndian(bufferSpan, articleId);

        var pos = 2;
        titleBytes.CopyTo(bufferSpan[pos..(titleBytes.Length + pos)]);

        pos += titleBytes.Length;
        contentBytes.CopyTo(bufferSpan[pos..(contentBytes.Length + pos)]);

        pos += contentBytes.Length;
        BinaryPrimitives.WriteUInt64BigEndian(bufferSpan[pos..(pos + sizeof(ulong))], DateCreated.ToEpoch());

        pos += sizeof(ulong);
        BinaryPrimitives.WriteUInt32BigEndian(bufferSpan[pos..(pos + sizeof(uint))], (uint)(Read ? 1 : 0));

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataNewsEntryArticle,
            Data = buffer,
        };
    }
}
