using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Persistence.Listeners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fragment.NetSlum.Persistence.Interceptors;

/// <summary>
/// Intercepts save changes calls in Entity Framework and dispatches the listener events for
/// any registered listeners that inherit types related to the update event
/// </summary>
public class EntityChangeInterceptor : SaveChangesInterceptor
{
    private readonly IEnumerable<IEntityChangeListener> _listeners;

    public EntityChangeInterceptor(IEnumerable<IEntityChangeListener> listeners)
    {
        _listeners = listeners;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is { } dbContext)
        {
            ExecuteListeners(dbContext);
        }

        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is { } dbContext)
        {
            ExecuteListeners(dbContext);
        }

        return new ValueTask<InterceptionResult<int>>(result);
    }

    private void ExecuteListeners(DbContext context)
    {
        context.ChangeTracker.DetectChanges();

        var changedEntities = context.ChangeTracker.Entries().ToArray();

        foreach (var entry in changedEntities)
        {
            var compatibleListeners = _listeners
                .Where(l => (l.GetType().BaseType?.GetGenericArguments()[0]!).IsInstanceOfType(context) &&
                            (l.GetType().BaseType?.GetGenericArguments()[1] == typeof(object) ||
                             (l.GetType().BaseType?.GetGenericArguments()[1].IsInstanceOfType(entry.Entity) ?? false)));

            foreach (var listener in compatibleListeners)
            {
                listener.EntityChanged(context, entry);
            }
        }
    }
}
