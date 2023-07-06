using Fragment.NetSlum.Core.Constants;

namespace Fragment.NetSlum.Server.Api.Models;

public class PlayerInfo
{
    public int Id { get; set; }
    public string CharacterName { get; set; } = default!;
    public CharacterClass Class { get; set; }
    public ushort? GuildId { get; set; }
    public ushort Level { get; set; }
    public string Greeting { get; set; } = default!;
    public ushort CurrentHp { get; set; }
    public ushort CurrentSp { get; set; }
    public uint CurrentGp { get; set; }
    public ushort OnlineTreasures { get; set; }
    public ushort AverageFieldLevel { get; set; }
    public ushort GoldCoinCount { get; set; }
    public ushort SilverCoinCount { get; set; }
    public ushort BronzeCoinCount { get; set; }
    public uint ModelId { get; set; }
    public string AvatarId { get; set; }
}
