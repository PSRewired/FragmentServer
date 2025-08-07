using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Persistence.Contexts;

/// <summary>
/// Represents the state of an entity during a change event
/// </summary>
/// <param name="Entity">The actual entity object</param>
/// <param name="State">The current <see cref="EntityState"/> that the entity is in</param>
/// <param name="OldValues">Represents the previous values of the entity. Null when <see cref="State"/> is <see cref="EntityState.Added"/> </param>
/// <param name="NewValues">Represents the current values of the entity. Null when <see cref="State"/> is <see cref="EntityState.Deleted"/> </param>
public record EntityChangeSnapshot(object Entity, EntityState State, ReadOnlyDictionary<string, object?>? OldValues, ReadOnlyDictionary<string, object?>? NewValues);
