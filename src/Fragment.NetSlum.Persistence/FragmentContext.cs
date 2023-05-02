#nullable disable

using System;
using System.Linq;
using Fragment.NetSlum.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            var entityBuilderType = typeof(EntityTypeBuilder<>).MakeGenericType(entityType);
            var entityBuilderObject = Activator.CreateInstance(entityBuilderType, modelBuilder.Entity(entityType).Metadata);

            var method = entityType.GetInterfaceMap(genericType).InterfaceMethods.First();

            method.Invoke(eInstance, new[] { entityBuilderObject });
        }
    }

    public virtual DbSet<Character> Characters { get; set; }
    public virtual DbSet<CharacterStats> CharacterStats { get; set; }
    public virtual DbSet<PlayerAccount> PlayerAccounts { get; set; }
    public virtual DbSet<CharacterStatHistory> CharacterStatHistory { get; set; }
    public virtual DbSet<ServerNews> ServerNews { get; set; }
}
