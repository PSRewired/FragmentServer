using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fragment.NetSlum.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Listeners;

/// <summary>
/// A change listener that will automatically generate history events when applicable
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="THistory"></typeparam>
public abstract class AbstractHistoryChangeListener<TEntity, THistory> : AbstractEntityChangeListener<FragmentContext, TEntity>
    where TEntity : class
{
    protected override Task OnEntityChanging(FragmentContext context, EntityChangeSnapshot snapshot)
    {
        if (snapshot.State is not EntityState.Modified && snapshot.OldValues?.Count < 1)
        {
            return Task.CompletedTask;
        }

        var changedValues = snapshot.OldValues?.Where(e =>
            {
                var compareNew = snapshot.NewValues?[e.Key]?.Equals(snapshot.OldValues?[e.Key]);
                var compareOld = snapshot.OldValues?[e.Key]?.Equals(snapshot.NewValues?[e.Key]);

                return (compareNew ?? compareOld) == false;
            })
            .Select(c => c.Key)
            .ToArray() ?? [];

        if (snapshot.OldValues == null || changedValues.Length == 0)
        {
            return Task.CompletedTask;
        }

        var historyEntryProperties = typeof(THistory).GetProperties().Select(p => p.Name).ToList();

        // If none of the values in the history entry have changed, ignore it.
        if (!changedValues.Any(c => historyEntryProperties.Contains(c, StringComparer.InvariantCultureIgnoreCase)))
        {
            return Task.CompletedTask;
        }

        var historyEntry = CreateHistoryEntry(snapshot);

        var validProperties = historyEntry!.GetType().GetProperties()
            .Where(p => p.GetCustomAttribute<KeyAttribute>() == null && p.GetCustomAttribute<DatabaseGeneratedAttribute>() == null);

        // Map all non-key properties
        foreach (var entityProperty in validProperties)
        {
            if (!snapshot.OldValues.TryGetValue(entityProperty.Name, out var oldValue))
            {
                continue;
            }

            entityProperty.SetValue(historyEntry, oldValue);
        }

        context.Add(historyEntry);

        return Task.CompletedTask;
    }

    protected override Task OnEntityChanged(FragmentContext context, EntityChangeSnapshot snapshot)
    {
        return Task.CompletedTask;
    }

    protected abstract THistory CreateHistoryEntry(EntityChangeSnapshot snapshot);
}
