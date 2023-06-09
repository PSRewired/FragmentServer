using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.BBS;

public class BbsThreadPostEntryResponse : BaseResponse
{
    private uint _postId;
    private DateTime _postedAtDate;

    private string _postedByUsername = null!;
    private string _postTitle = null!;

    public BbsThreadPostEntryResponse SetPostId(uint id)
    {
        _postId = id;

        return this;
    }

    public BbsThreadPostEntryResponse SetPostedAt(DateTime date)
    {
        _postedAtDate = date;

        return this;
    }

    public BbsThreadPostEntryResponse SetPostedByUsername(string username)
    {
        _postedByUsername = username;

        return this;
    }

    public BbsThreadPostEntryResponse SetPostTile(string title)
    {
        _postTitle = title;

        return this;
    }

    public override FragmentMessage Build()
    {
        uint unk0 = _postId;
        uint unk2 = 0;
        var unk3 = new Span<byte>(new byte[96]);

        var usernameBytes = _postedByUsername.ToShiftJis().EnsureSize(32);
        var titleBytes = _postTitle.ToShiftJis().EnsureSize(128);

        var titlePreview = new Span<byte>(new byte[17]);
        titleBytes[..16].CopyTo(titlePreview);

        var writer = new MemoryWriter(sizeof(uint) * 4 +
                                      usernameBytes.Length +
                                      titleBytes.Length +
                                      titlePreview.Length +
                                      unk3.Length
        );

        writer.Write(unk0);
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
            DataPacketType = OpCodes.DataBbsThreadPostEntryInfoResponse,
            Data = writer.Buffer,
        };
    }
}
