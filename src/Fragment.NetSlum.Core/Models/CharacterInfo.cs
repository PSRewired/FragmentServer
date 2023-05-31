using System.Buffers.Binary;
using System.Text;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;

namespace Fragment.NetSlum.Core.Models;

public class CharacterInfo
{
    public byte SaveSlot { get; set; }
    public string SaveId { get; set; } = default!;
    public string CharacterName { get; set; } = default!;
    public CharacterClass Class { get; set; }
    public ushort Level { get; set; }
    public string Greeting { get; set; } = default!;
    public uint Model { get; set; }
    public ushort CurrentHp { get; set; }
    public ushort CurrentSp { get; set; }
    public uint CurrentGp { get; set; }
    public ushort OnlineStatueCounter { get; set; }
    public ushort OfflineStatueCounter { get; set; }
    public ushort GoldCoinCount { get; set; }
    public ushort SilverCoinCount { get; set; }
    public ushort BronzeCoinCount { get; set; }
    public uint ModelId { get; set; }
    public char ModelType => GetModelType(ModelId);
    public CharacterColor Color => GetModelColor(ModelId);

    public string ModelFile => GetModelFileName();
    public char ModelClass => GetModelClass(ModelId);
    public int ModelNumber => GetModelNumber(ModelId);

    public static CharacterInfo FromBinaryData(Span<byte> data)
    {
        var pos = 1;

        var saveSlot = data[0];

        var saveId = data[pos..].GetNullTerminatedString();
        pos += saveId.Length;

        var characterName = data[pos..].ReadToNullByte();
        pos += characterName.Length;

        var characterClass = (CharacterClass) data[pos];
        pos += 1;
        var characterLevel = BinaryPrimitives.ReadUInt16BigEndian(data[pos..(pos + 2)]);
        pos += 2;

        var characterGreeting = data[pos..].ReadToNullByte();
        pos += characterGreeting.Length;

        var characterModel = BinaryPrimitives.ReadUInt32BigEndian(data[pos..(pos + 4)]);
        pos += 5;

        var currentHp = BinaryPrimitives.ReadUInt16BigEndian(data[pos..(pos + 2)]);
        pos += 2;
        var currentSp = BinaryPrimitives.ReadUInt16BigEndian(data[pos..(pos + 2)]);
        pos += 2;
        var currentGp = BinaryPrimitives.ReadUInt32BigEndian(data[pos..(pos + 4)]);
        pos += 4;

        var godOnline = BinaryPrimitives.ReadUInt16BigEndian(data[pos..(pos + 2)]);
        pos += 2;
        var godOffline = BinaryPrimitives.ReadUInt16BigEndian(data[pos..(pos + 2)]);
        pos += 2;

        var goldCoinCount = BinaryPrimitives.ReadUInt16BigEndian(data[pos..(pos + 2)]);
        pos += 2;

        var silverCoinCount = BinaryPrimitives.ReadUInt16BigEndian(data[pos..(pos + 2)]);
        pos += 2;

        var bronzeCoinCount = BinaryPrimitives.ReadUInt16BigEndian(data[pos..(pos + 2)]);

        return new CharacterInfo
        {
            SaveSlot = saveSlot,
            SaveId = saveId,
            BronzeCoinCount = bronzeCoinCount,
            CharacterName = characterName.ToShiftJisString(),
            Class = characterClass,
            Greeting = characterGreeting.ToShiftJisString(),
            Level = characterLevel,
            Model = characterModel,
            CurrentGp = currentGp,
            CurrentHp = currentHp,
            CurrentSp = currentSp,
            GoldCoinCount = goldCoinCount,
            SilverCoinCount = silverCoinCount,
            OfflineStatueCounter = godOffline,
            OnlineStatueCounter = godOnline,
            ModelId = characterModel,
        };
    }

    public override string ToString()
    {
        var sb = new StringBuilder("Character Info:");
        sb.AppendLine();

        foreach (var prop in GetType().GetProperties())
        {
            if (!prop.CanRead)
            {
                continue;
            }

            var propValue = prop.GetValue(this);
            sb.AppendLine($"{prop.Name}: {propValue?.ToString()}");
        }

        return sb.ToString();
    }

    private string GetModelFileName()
    {
        var classLetter = Class.ToString().ToLower()[0];

        return $"xf{classLetter}{ModelNumber}{ModelType}_{Color.ToColorCode()}";
    }

    private static char GetModelClass(uint modelNumber)
    {
        return ((CharacterClass) (modelNumber & 0x0F)).ToString().ToLower()[0];
    }

    private static int GetModelNumber(uint modelNumber)
    {
        return (int) ((modelNumber >> 4 & 0x0F) + 1);
    }

    private static char GetModelType(uint modelNumber)
    {
        return (char) (((modelNumber >> 12) & 0x0F) + 0x41);
    }

    private static CharacterColor GetModelColor(uint modelNumber)
    {
        return (CharacterColor) ((modelNumber >> 8) & 0x0F);
    }
}
