using System;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Server.Api.Models;

public class Client
{
    public int AccountId { get; set; }
    public int CharacterId { get; set; }
    public string? CharacterName { get; set; }
    public CharacterClass? Class { get; set; }
    public ushort? Level { get; set; }
    public ushort? CurrentHp { get; set; }
    public ushort? CurrentSp { get; set; }
    public uint? CurrentGp { get; set; }
    public ushort? GoldAmount { get; set; }
    public ushort? SilverAmount { get; set; }
    public ushort? BronzeAmount { get; set; }
    public uint? ModelId { get; set; }
    public string? AvatarId { get; set; }
    public DateTime? LastPing { get; set; }

    public static Client FromSession(FragmentTcpSession session)
    {
        return new Client
        {
            AccountId = session.PlayerAccountId,
            CharacterId = session.CharacterId,
            CharacterName = session.CharacterInfo?.CharacterName,
            Class = session.CharacterInfo?.Class,
            Level = session.CharacterInfo?.Level,
            CurrentHp = session.CharacterInfo?.CurrentHp,
            CurrentSp = session.CharacterInfo?.CurrentSp,
            CurrentGp = session.CharacterInfo?.CurrentGp,
            GoldAmount = session.CharacterInfo?.GoldCoinCount,
            SilverAmount = session.CharacterInfo?.SilverCoinCount,
            BronzeAmount = session.CharacterInfo?.BronzeCoinCount,
            ModelId = session.CharacterInfo?.ModelId,
            AvatarId = session.CharacterInfo?.ModelFile,
            LastPing = session.LastContacted,
        };
    }
}
