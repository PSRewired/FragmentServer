
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fragment.NetSlum.Persistence.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Persistence.Listeners;

/// <summary>
/// Listener that checks for [Timestampable] attributes on entity properties and updates their values before committing to the database.
/// </summary>
public class TimestampableEntityListener : AbstractEntityChangeListener<DbContext, object>
{
    private readonly ILogger<TimestampableEntityListener> _logger;

    public TimestampableEntityListener(ILogger<TimestampableEntityListener> logger)
    {
        _logger = logger;
    }

    protected override Task OnEntityChanged(DbContext context, EntityEntry entry)
    {
        var timestampables = entry.Entity.GetType().GetProperties()
            .Where(p => p.GetCustomAttribute<TimestampableAttribute>() != null)
            .Select(property => new
            {
                Property = property,
                States = property.GetCustomAttribute<TimestampableAttribute>()!.States,
            })
            .Where(property => Array.Exists(property.States, e => e == entry.State))
            .ToDictionary(k => k.Property, v => v.States);

        if (!timestampables.Any())
        {
            return Task.CompletedTask;
        }

        var now = DateTime.UtcNow;

        foreach (var (property, states) in timestampables)
        {
            if (!IsValidFieldType(property))
            {
                throw new DbUpdateException(
                    $"Property {property.Name} on entity {entry.Metadata.Name} must be of type {typeof(DateTime)} or {typeof(DateTime?)}");
            }

            _logger.LogDebug("[{ClsName}] Updating timestampable property {PropertyName} ({StatesListened}) on {EntityName} to {NewValue:u} for state change of {EntityState}",
                GetType().Name, property.Name, string.Join(',', states), entry.Metadata.Name, now, entry.State);

            property.SetValue(entry.Entity, now);
        }

        return Task.CompletedTask;
    }

    private static bool IsValidFieldType(PropertyInfo property)
    {
        return property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?);
    }
}
