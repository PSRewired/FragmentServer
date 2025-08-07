using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fragment.NetSlum.Persistence.Contexts;
using Fragment.NetSlum.Persistence.Listeners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Fragment.NetSlum.Persistence.Interceptors;

/// <summary>
/// Intercepts save changes calls in Entity Framework and dispatches the listener events for
/// any registered listeners that inherit types related to the update event
/// </summary>
public class EntityChangeInterceptor : SaveChangesInterceptor
{
    private readonly IEnumerable<IEntityChangedListener> _listeners;

    private record EntityContext(EntityEntry Entry, EntityChangeSnapshot Snapshot);

    private readonly ConcurrentQueue<EntityContext> _changingEntities = new();

    public EntityChangeInterceptor(IEnumerable<IEntityChangedListener> listeners)
    {
        _listeners = listeners;
    }

    /// <inheritdoc />
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not { } dbContext)
        {
            return result;
        }

        GenerateEntityEvents(dbContext);

        ExecuteListeners(dbContext, _changingEntities.ToArray(),
                (listener, context, entity) => listener.EntityChanging(context, entity))
            .Wait();

        return result;
    }

    /// <inheritdoc />
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is not { } dbContext)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        GenerateEntityEvents(dbContext);

        await ExecuteListeners(dbContext, _changingEntities.ToArray(),
            (listener, context, entity) => listener.EntityChanging(context, entity));

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <inheritdoc />
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        if (eventData.Context is not { } dbContext)
        {
            return base.SavedChanges(eventData, result);
        }

        ExecuteListeners(dbContext, GetPendingSnapshots(),
                (listener, context, snapshot) => listener.EntityChanged(context, snapshot))
            .Wait();

        return base.SavedChanges(eventData, result);
    }

    /// <inheritdoc />
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is not { } dbContext)
        {
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        await ExecuteListeners(dbContext, GetPendingSnapshots(),
            (listener, context, snapshot) => listener.EntityChanged(context, snapshot));

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void GenerateEntityEvents(DbContext context)
    {
        var entityEntries = GetChangedEntities(context);

        foreach (var entry in entityEntries)
        {
            _changingEntities.Enqueue(new EntityContext(entry, CreateSnapshot(entry)));
        }
    }

    private static EntityChangeSnapshot CreateSnapshot(EntityEntry entry)
    {
        var snapshot = new EntityChangeSnapshot(entry.Entity, entry.State, null, null);

        return HydrateSnapshot(entry, snapshot);
    }

    private EntityContext[] GetPendingSnapshots()
    {
        var snapshots = new EntityContext[_changingEntities.Count];
        var i = 0;

        while (_changingEntities.TryDequeue(out var entity))
        {
            snapshots[i++] = entity with { Snapshot = HydrateSnapshot(entity.Entry, entity.Snapshot) };
        }

        return snapshots;
    }

    private static EntityChangeSnapshot HydrateSnapshot(EntityEntry entry, EntityChangeSnapshot snapshot)
    {
        switch (snapshot.State)
        {
            case EntityState.Modified:
                return snapshot with
                {
                    OldValues = snapshot.OldValues ?? BuildPropertyDictionary(entry.OriginalValues),
                    NewValues = BuildPropertyDictionary(entry.CurrentValues)
                };
            case EntityState.Added:
                return snapshot with { OldValues = null, NewValues = BuildPropertyDictionary(entry.CurrentValues) };
            default:
                return snapshot;
        }
    }

    private static IEnumerable<EntityEntry> GetChangedEntities(DbContext context)
    {
        context.ChangeTracker.DetectChanges();

        return context.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted &&
                        (!e.Entity.GetType().IsGenericType || e.Entity.GetType().GetGenericTypeDefinition() != typeof(Dictionary<,>)))
            .ToArray();
    }

    private async Task ExecuteListeners(DbContext context, IEnumerable<EntityContext> entities,
        Func<IEntityChangedListener, DbContext, EntityChangeSnapshot, Task> listenerMethod)
    {
        foreach (var entry in entities)
        {
            var compatibleListeners = _listeners
                .Where(l => (l.GetType().BaseType?.GetGenericArguments()[0]!).IsInstanceOfType(context) &&
                            (l.GetType().BaseType?.GetGenericArguments()[1] == typeof(object) ||
                             (l.GetType().BaseType?.GetGenericArguments()[1].IsInstanceOfType(entry.Entry.Entity) ??
                              false)));

            foreach (var listener in compatibleListeners)
            {
                await listenerMethod(listener, context, entry.Snapshot);
            }
        }
    }

    private static ReadOnlyDictionary<string, object?>? BuildPropertyDictionary(PropertyValues? values)
    {
        var entry = values?.ToObject();
        return values?.Properties.ToDictionary(k => k.Name, v => v.PropertyInfo?.GetValue(entry)).AsReadOnly();
    }
}
