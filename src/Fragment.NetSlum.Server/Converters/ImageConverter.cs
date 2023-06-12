using System;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Core.Models;
using ImageMagick;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Server.Converters;

public class ImageConverter
{
    private readonly ILogger<ImageConverter> _logger;

    //private const int ColorMapLength = 768;
    public ImageConverter(ILogger<ImageConverter> logger)
    {
        _logger = logger;
    }

    public ImageInfo Convert(byte[] imageBytes)
    {
        using var image = new MagickImage(imageBytes)
        {
            Format = MagickFormat.Tga,
            ColorType = ColorType.Palette,
            ColorSpace = ColorSpace.RGB,
            Depth = 32,
            Orientation = OrientationType.TopLeft,
            Comment = null,
            Quality = 20,
        };
        // Ensure the image is always 128x128 otherwise it gets corrupted in-game
        image.Resize(128, 128);

        var convertedImage = image.ToByteArray().AsMemory();
        image.Write("test.tga");

        _logger.LogDebug("Converted image data:\n{HexDump}", convertedImage.ToHexDump());
        //MagickNET.SetLogEvents(LogEvents.All);
        //MagickNET.Log += (s, a) => _logger.LogDebug("{Type}: {Message}", a.EventType, a.Message);

        //var colorMapLength = image.ColormapSize;

        // Ensure origin TopLeft is set since ImageMagick doesn't seem to set this bit
        //convertedImage.Span[0x11] = 0x08;

        return new ImageInfo
        {
            //ColorData = convertedImage[..(18+colorMapLength)],
            //ImageData = convertedImage[(18+colorMapLength)..],
            ImageData = convertedImage,
        };
    }
}
