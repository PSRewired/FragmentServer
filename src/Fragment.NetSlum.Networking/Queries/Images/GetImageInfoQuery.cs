using Fragment.NetSlum.Core.CommandBus.Contracts.Queries;
using Fragment.NetSlum.Core.Models;

namespace Fragment.NetSlum.Networking.Queries.Images;

public class GetImageInfoQuery : IQuery<ImageInfo>
{
    public byte[] ImageData { get; }

    public GetImageInfoQuery(byte[] imageData)
    {
        ImageData = imageData;
    }
}
