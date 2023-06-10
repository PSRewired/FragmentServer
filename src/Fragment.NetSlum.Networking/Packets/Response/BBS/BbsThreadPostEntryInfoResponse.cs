using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.BBS;

public class BbsThreadPostDetailResponse : BaseResponse
{
    private uint _threadId;
    private uint _postId;
    private DateTime _postedAtDate;

    private string _postedByUsername = null!;
    private string _postTitle = null!;

    public BbsThreadPostDetailResponse SetThreadId(uint id)
    {
        _threadId = id;

        return this;
    }

    public BbsThreadPostDetailResponse SetPostId(uint id)
    {
        _postId = id;

        return this;
    }

    public BbsThreadPostDetailResponse SetPostedAt(DateTime date)
    {
        _postedAtDate = date;

        return this;
    }

    public BbsThreadPostDetailResponse SetPostedByUsername(string username)
    {
        _postedByUsername = username;

        return this;
    }

    public BbsThreadPostDetailResponse SetPostTile(string title)
    {
        _postTitle = title;

        return this;
    }

    public override FragmentMessage Build()
    {
        uint unk0 = _postId;
        uint unk2 = 0;
        var unk3 = new Span<byte>(new byte[96]);

        var usernameBytes = _postedByUsername.MaxSubstring(15).ToShiftJis().EnsureSize(16);
        var titleBytes = _postTitle.MaxSubstring(31).ToShiftJis().EnsureSize(32);
        var titlePreview = _postTitle.MaxSubstring(16).ToShiftJis().EnsureSize(17);

        var writer = new MemoryWriter(sizeof(uint) * 4 +
                                      usernameBytes.Length +
                                      titleBytes.Length +
                                      titlePreview.Length +
                                      unk3.Length
        );

        writer.Write(unk0); // This appears to be whether or not the user has read the post or not. (If value >0 then 'NEW' does not appear)
        writer.Write(_postId);
        writer.Write(unk2);
        writer.Write((uint)_postedAtDate.ToEpoch());
        writer.Write(usernameBytes);
        writer.Write(titlePreview);
        writer.Write(unk3);
        writer.Write(titleBytes);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataBbsThreadPostEntryDetailResponse,
            Data = writer.Buffer,
        };
    }
}
