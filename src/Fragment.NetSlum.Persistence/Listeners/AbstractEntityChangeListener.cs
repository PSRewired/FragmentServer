using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fragment.NetSlum.Persistence.Listeners;

public abstract class AbstractEntityChangeListener<TContext, TEntity> : IEntityChangeListener where TEntity : class where TContext : DbContext
{
    protected abstract Task OnEntityChanged(TContext context, EntityEntry entry);

    public Task EntityChanged(DbContext context, EntityEntry entry)
    {
        if (context is not TContext expectedContext)
        {
            throw new ArgumentException($"Invalid context {context.GetType()} encountered in {GetType().Name}. Expected {typeof(TContext).Name}");
        }

        if (entry.Entity is not TEntity)
        {
            throw new ArgumentException($"Invalid entity {entry.Entity.GetType()} encountered in {GetType().Name}. Expected {typeof(TEntity).Name}");
        }

        return OnEntityChanged(expectedContext, entry);
    }
}
