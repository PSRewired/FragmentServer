using System;

namespace Fragment.NetSlum.Server.Api.Models;

public class PlayerStats
{
    public ushort Level { get; set; }
    public ushort CurrentHp { get; set; }
    public ushort CurrentSp { get; set; }
    public uint CurrentGp { get; set; }
    public ushort OnlineTreasures { get; set; }
    public ushort AverageFieldLevel { get; set; }
    public ushort GoldCoinCount { get; set; }
    public ushort SilverCoinCount { get; set; }
    public ushort BronzeCoinCount { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
