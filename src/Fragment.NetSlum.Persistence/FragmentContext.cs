#nullable disable

using System;
using System.Linq;
using Fragment.NetSlum.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Fragment.NetSlum.Persistence;

public class FragmentContext : DbContext
{
    public FragmentContext()
    {
    }

    public FragmentContext(DbContextOptions<FragmentContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use snake_cased table/column names to keep things consistent with the old database layout
        optionsBuilder
            .UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var configurableEntityTypes = GetType().Assembly.GetTypes().Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(IConfigurableEntity<>).MakeGenericType(t)));

        foreach (var entityType in configurableEntityTypes)
        {
            var genericType = typeof(IConfigurableEntity<>).MakeGenericType(entityType);
            var eInstance = Activator.CreateInstance(entityType);

            var booty = CastToType(genericType, modelBuilder.Entity(entityType));
            var method = entityType.GetInterfaceMap(genericType).InterfaceMethods.First();

            method.Invoke(eInstance, new object[] { booty });
        }
    }

#pragma warning disable IDE0060
    private static T CastToType<T>(T _, object instance) where T : class
    {
        return instance as T;
    }
#pragma warning enable IDE0060

    public virtual DbSet<Character> Characters { get; set; }
    public virtual DbSet<CharacterCurrency> CharacterCurrencies { get; set; }
    public virtual DbSet<CharacterStats> CharacterStats { get; set; }
    public virtual DbSet<PlayerAccount> PlayerAccounts { get; set; }
}
