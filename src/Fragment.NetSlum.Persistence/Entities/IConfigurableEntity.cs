using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fragment.NetSlum.Persistence.Entities;

/// <summary>
/// Allows for extended entity configuration during the model creation phase of the context
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IConfigurableEntity<TEntity> where TEntity : class
{
    /// <summary>
    /// Allows for additional model configuration that may not be available via attributes.
    /// This method is executed during <see cref="FragmentContext.OnModelCreating"/>
    /// </summary>
    /// <param name="entityBuilder"></param>
    internal void Configure(EntityTypeBuilder<TEntity> entityBuilder);
}
