using System.Threading.Tasks;
using Fragment.NetSlum.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fragment.NetSlum.Persistence.Listeners;

public class CharacterStatsChangeListener : AbstractEntityChangeListener<FragmentContext, CharacterStats>
{
    protected override Task OnEntityChanged(FragmentContext context, EntityEntry entry)
    {
        if (entry.Entity is not CharacterStats statsEntry)
        {
            return Task.CompletedTask;
        }

        // If the player stats are deleted (which should never happen), we can ignore stats updates
        if (entry.State == EntityState.Deleted)
        {
            return Task.CompletedTask;
        }

        context.CharacterStatHistory.Add(CharacterStatHistory.FromStats(statsEntry));

        return Task.CompletedTask;
    }
}
