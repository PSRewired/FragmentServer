
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fragment.NetSlum.Persistence.Attributes;
using Fragment.NetSlum.Persistence.Contexts;
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

    protected override Task OnEntityChanged(DbContext context, EntityChangeSnapshot snapshot)
    {
        return Task.CompletedTask;
    }

    protected override Task OnEntityChanging(DbContext context, EntityChangeSnapshot snapshot)
    {
        var timestampables = snapshot.Entity.GetType().GetProperties()
            .Where(p => p.GetCustomAttribute<TimestampableAttribute>() != null)
            .Select(property => new
            {
                Property = property,
                States = property.GetCustomAttribute<TimestampableAttribute>()!.States,
            })
            .Where(property => Array.Exists(property.States, e => e == snapshot.State))
            .ToDictionary(k => k.Property, v => v.States);

        if (!timestampables.Any())
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
            .ToArray() ?? Array.Empty<string>();

        // Compare all changed values to the list of timestampables. If nothing has changed other than modified timestampable
        // properties, do not update the timestamped values.
        var hasChangesOtherThanModifiedStampables = snapshot.State != EntityState.Modified || changedValues
            .Any(c =>
            {
                var ts = timestampables.FirstOrDefault(t => t.Key.Name == c);
                return ts.Equals(default(KeyValuePair<PropertyInfo, EntityState[]>));
            });


        if (!hasChangesOtherThanModifiedStampables)
        {
            return Task.CompletedTask;
        }

        var now = DateTime.UtcNow;

        foreach (var (property, states) in timestampables)
        {
            if (!IsValidFieldType(property))
            {
                throw new DbUpdateException(
                    $"Property {property.Name} on entity {snapshot.Entity.GetType().Name} must be of type {typeof(DateTime)} or {typeof(DateTime?)}");
            }

            var ignoredProperties = snapshot.Entity.GetType().GetCustomAttributes<TimestampableIgnorePropertyAttribute>()
                .Concat(property.GetCustomAttributes<TimestampableIgnorePropertyAttribute>())
                .SelectMany(attr => attr.PropertyNames);

            if (changedValues.Length > 0 && changedValues.All(cv => ignoredProperties.Contains(cv)))
            {
                _logger.LogDebug("Timestampable property {PropertyName} update ignored because the only updates were to ignored values", property.Name);
                continue;
            }

            _logger.LogDebug("Updating timestampable property {PropertyName} ({StatesListened}) on {EntityName} to {NewValue:u} for state change of {EntityState}",
                property.Name, string.Join(',', states), snapshot.Entity.GetType().Name, now, snapshot.State);

            property.SetValue(snapshot.Entity, now);
        }

        return Task.CompletedTask;
    }

    private static bool IsValidFieldType(PropertyInfo property)
    {
        return property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?);
    }
}
