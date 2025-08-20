using System;
using System.Threading.Tasks;
using Fragment.NetSlum.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Listeners;

public abstract class AbstractEntityChangeListener<TContext, TEntity> : IEntityChangedListener
    where TEntity : class where TContext : DbContext
{
    protected virtual Task OnEntityChanged(TContext context, EntityChangeSnapshot snapshot)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnEntityChanging(TContext context, EntityChangeSnapshot snapshot)
    {
        return Task.CompletedTask;
    }

    public Task EntityChanging(DbContext context, EntityChangeSnapshot snapshot)
    {
        if (context is not TContext expectedContext)
        {
            throw new ArgumentException(
                $"Invalid context {context.GetType()} encountered in {GetType().Name}. Expected {typeof(TContext).Name}");
        }

        if (snapshot.Entity is not TEntity)
        {
            throw new ArgumentException(
                $"Invalid entity {snapshot.Entity.GetType()} encountered in {GetType().Name}. Expected {typeof(TEntity).Name}");
        }

        return OnEntityChanging(expectedContext, snapshot);
    }

    public Task EntityChanged(DbContext context, EntityChangeSnapshot snapshot)
    {
        if (context is not TContext expectedContext)
        {
            throw new ArgumentException(
                $"Invalid context {context.GetType()} encountered in {GetType().Name}. Expected {typeof(TContext).Name}");
        }

        if (snapshot.Entity is not TEntity)
        {
            throw new ArgumentException(
                $"Invalid entity {snapshot.Entity.GetType()} encountered in {GetType().Name}. Expected {typeof(TEntity).Name}");
        }

        return OnEntityChanged(expectedContext, snapshot);
    }
}
