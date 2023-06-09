using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.BBS;

public class BbsPostContentResponse : BaseResponse
{
    private int _postId;
    private string _postContent = null!;

    public BbsPostContentResponse SetPostId(int id)
    {
        _postId = id;

        return this;
    }

    public BbsPostContentResponse SetContent(string content)
    {
        _postContent = content;

        return this;
    }

    public override FragmentMessage Build()
    {
        var contentBytes = _postContent.ToShiftJis();

        var writer = new MemoryWriter(contentBytes.Length + sizeof(int));

        writer.Write(_postId);
        writer.Write(contentBytes);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataBbsPostContentResponse,
            Data = writer.Buffer,
        };
    }
}
