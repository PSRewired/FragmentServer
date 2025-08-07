using Fragment.NetSlum.Persistence.Contexts;
using Fragment.NetSlum.Persistence.Entities;

namespace Fragment.NetSlum.Persistence.Listeners;

public class CharacterStatsChangeListener : AbstractHistoryChangeListener<CharacterStats, CharacterStatHistory>
{
    protected override CharacterStatHistory CreateHistoryEntry(EntityChangeSnapshot snapshot)
    {
        return CharacterStatHistory.FromStats((CharacterStats)snapshot.Entity);
    }
}
