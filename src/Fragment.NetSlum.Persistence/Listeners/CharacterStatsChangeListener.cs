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

        // Only update the history tables when stats are modified or added for the first time
        if (entry.State != EntityState.Modified || entry.State != EntityState.Added)
        {
            return Task.CompletedTask;
        }

        context.CharacterStatHistory.Add(CharacterStatHistory.FromStats(statsEntry));

        return Task.CompletedTask;
    }
}
