using System;
using System.IO;
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

    public ImageInfo ConvertTga(byte[] imageBytes)
    {
        byte[] convertedImage;
        try
        {
            //MagickNET.SetLogEvents(LogEvents.All);
            //MagickNET.Log += (s, a) => _logger.LogDebug("{Type}: {Message}", a.EventType, a.Message);

            using var image = new MagickImage(imageBytes)
            {
                Format = MagickFormat.Tga,
                ColorType = ColorType.Palette,
                Depth = 32,
                Orientation = OrientationType.TopLeft,
                Comment = null,
                //Quality = 20,
            };
            // Ensure the image is always 128x128 otherwise it gets corrupted in-game
            image.Resize(new MagickGeometry(128, 128) { IgnoreAspectRatio = true });

            _logger.LogInformation("Image Info: {ColorMapSize}", image.ColormapSize);

            // Fragment appears to only support images that have a max colormap size of 768
            if (image.ColormapSize > 768)
            {
                image.ColormapSize = 768;
            }

            image.Write("test.tga");
            convertedImage = image.ToByteArray();
        }
        catch (MagickMissingDelegateErrorException) // Assume if conversion fails, this image is already formatted
        {
            convertedImage = imageBytes;
        }

        return new ImageInfo
        {
            ImageData = convertedImage,
        };
    }

    public ImageInfo ConvertPng(byte[] imageBytes)
    {
        byte[] convertedImage;
        try
        {
            //MagickNET.SetLogEvents(LogEvents.All);
            //MagickNET.Log += (s, a) => _logger.LogDebug("{Type}: {Message}", a.EventType, a.Message);

            using var image = new MagickImage(imageBytes)
            {
                Format = MagickFormat.Png,
            };
            // Ensure the image is always 128x128 otherwise it gets corrupted in-game
            image.Resize(new MagickGeometry(128, 128) { IgnoreAspectRatio = true });

            _logger.LogInformation("Image Info: {ColorMapSize}", image.ColormapSize);

            convertedImage = image.ToByteArray();
        }
        catch (MagickMissingDelegateErrorException) // Assume if conversion fails, this image is already formatted
        {
            convertedImage = imageBytes;
        }

        return new ImageInfo
        {
            ImageData = convertedImage,
        };
    }
}
