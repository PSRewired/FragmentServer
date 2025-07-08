using System;
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
        uint unk2 = 0;
        var unk3 = new Span<byte>(new byte[46]);

        var usernameBytes = _postedByUsername.MaxSubstring(15).ToShiftJis().EnsureSize(16);
        var titleBytes = _postTitle.MaxSubstring(31).ToShiftJis().EnsureSize(32);
        var titlePreview = _postTitle.MaxSubstring(16).ToShiftJis().EnsureSize(18);

        var writer = new MemoryWriter(sizeof(uint) * 4 +
                                      usernameBytes.Length +
                                      titleBytes.Length +
                                      titlePreview.Length +
                                      unk3.Length
        );

        writer.Write(_postId); //If this number isn't unique in a thread then only the latest post is going to show for the whole thread.
        writer.Write(_postId);
        writer.Write(unk2);
        writer.Write((uint)_postedAtDate.ToEpoch());
        writer.Write(usernameBytes);
        writer.Write(titlePreview);
        writer.Write(unk3);
        writer.Write(titleBytes);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataBbsThreadPostEntryDetailResponse,
            Data = writer.Buffer,
        };
    }
}
