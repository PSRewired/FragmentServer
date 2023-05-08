namespace Fragment.NetSlum.Core.Models;

public struct ImageInfo
{
    public ImageInfo()
    {
    }

    public ushort ChunkCount => 1;
    public uint ImageSize => (uint)(ColorData.Length + ImageData.Length);

    public Memory<byte> ColorData { get; set; } = Array.Empty<byte>();
    public Memory<byte> ImageData { get; set; } = Array.Empty<byte>();
}
