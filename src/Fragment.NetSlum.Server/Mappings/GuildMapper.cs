// ReSharper disable RedundantVerbatimPrefix
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Server.Api.Models;
using Riok.Mapperly.Abstractions;

namespace Fragment.NetSlum.Server.Mappings;

[Mapper]
public static partial class GuildMapper
{
    [MapProperty(nameof(@Guild.Members.Count), nameof(GuildInfo.MemberCount))]
    [MapProperty(nameof(@Guild.Stats.GoldAmount), nameof(GuildInfo.GoldAmount))]
    [MapProperty(nameof(@Guild.Stats.SilverAmount), nameof(GuildInfo.SilverAmount))]
    [MapProperty(nameof(@Guild.Stats.BronzeAmount), nameof(GuildInfo.BronzeAmount))]
    [MapProperty(nameof(@Guild.Stats.CurrentGp), nameof(GuildInfo.CurrentGp))]
    [MapProperty(nameof(@Guild.Stats.UpdatedAt), nameof(GuildInfo.LastUpdatedAt))]
    public static partial GuildInfo Map(Guild guild);
}
