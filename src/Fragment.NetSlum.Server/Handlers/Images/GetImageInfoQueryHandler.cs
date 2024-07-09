using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus.Contracts.Queries;
using Fragment.NetSlum.Core.Models;
using Fragment.NetSlum.Networking.Queries.Images;
using Fragment.NetSlum.Server.Converters;

namespace Fragment.NetSlum.Server.Handlers.Images;

public class GetImageInfoQueryHandler : QueryHandler<GetImageInfoQuery, ImageInfo>
{
    private readonly ImageConverter _imageConverter;

    public GetImageInfoQueryHandler(ImageConverter imageConverter)
    {
        _imageConverter = imageConverter;
    }

    public override ValueTask<ImageInfo> Handle(GetImageInfoQuery command, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(_imageConverter.Convert(command.ImageData));
    }
}
