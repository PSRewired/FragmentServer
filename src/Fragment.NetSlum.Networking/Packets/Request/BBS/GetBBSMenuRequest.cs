using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.BBS;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Networking.Packets.Request.BBS;

[FragmentPacket(OpCodes.Data, OpCodes.DataBbsGetMenu)]
public class GetBBSMenuRequest : BaseRequest
{
    private readonly FragmentContext _database;

    public GetBBSMenuRequest(FragmentContext database)
    {
        _database = database;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        uint categoryId = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        if (categoryId == 0)
        {
            return Task.FromResult(GetBbcCategories());
        }

        var threads = _database.BbsThreads
            .AsNoTracking()
            .Where(t => t.CategoryId == categoryId);

        var responses = new List<FragmentMessage>();

        responses.Add(new BbsThreadCountResponse((ushort)threads.Count()).Build());

        foreach (var thread in threads)
        {
            responses.Add(new BbsThreadEntryResponse()
                .SetThreadId(thread.Id)
                .SetThreadTitle(thread.Title)
                .Build());
        }

        return Task.FromResult<ICollection<FragmentMessage>>(responses);
    }

    private ICollection<FragmentMessage> GetBbcCategories()
    {
        var responses = new List<FragmentMessage>();
        var availableCategories = _database.BbsCategories;

        responses.Add(new BbsCategoryCountResponse((ushort)availableCategories.Count()).Build());

        foreach (var category in availableCategories)
        {
            responses.Add(new BbsCategoryEntryResponse()
                .SetCategoryId(category.Id)
                .SetCategoryTitle(category.CategoryName)
                .Build());
        }

        return responses;
    }
}
