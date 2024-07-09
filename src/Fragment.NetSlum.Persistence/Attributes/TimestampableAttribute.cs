using System;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Attributes;

/// <summary>
/// Denotes a DateTime property that should be updated on an entity state change
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class TimestampableAttribute : Attribute
{
    public EntityState[] States { get; }

    /// <summary>
    /// By default, the property will be updated on Added and Modified states
    /// </summary>
    public TimestampableAttribute()
    {
        States = [EntityState.Added, EntityState.Modified];
    }

    /// <summary>
    /// Explicitly set the states of when to modify this property's value
    /// </summary>
    /// <param name="states">
    /// The list of states to listen for
    /// </param>
    public TimestampableAttribute(params EntityState[] states)
    {
        States = states;
    }
}
