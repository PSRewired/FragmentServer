#nullable disable

using System;
using System.Linq;
using EntityFramework.Exceptions.MySQL.Pomelo;
using Fragment.NetSlum.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fragment.NetSlum.Persistence;

//dotnet ef migrations add Initial -s .\src\Fragment.NetSlum.Server\ -p .\src\Fragment.NetSlum.Persistence\ --context FragmentContext
public class FragmentContext : DbContext
{
    public FragmentContext()
    {
    }

    public FragmentContext(DbContextOptions<FragmentContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use snake_cased table/column names to keep things consistent with the old database layout
        optionsBuilder
            .UseSnakeCaseNamingConvention()
            .UseExceptionProcessor();

        #if (DEBUG)
        optionsBuilder.EnableSensitiveDataLogging();
        #endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var configurableEntityTypes = GetType().Assembly.GetTypes().Where(t => !t.IsAbstract && t.IsClass && t.IsAssignableTo(typeof(IConfigurableEntity<>).MakeGenericType(t)));

        foreach (var entityType in configurableEntityTypes)
        {
            var genericType = typeof(IConfigurableEntity<>).MakeGenericType(entityType);
            var eInstance = Activator.CreateInstance(entityType);

            var entityBuilderType = typeof(EntityTypeBuilder<>).MakeGenericType(entityType);
            var entityBuilderObject = Activator.CreateInstance(entityBuilderType, modelBuilder.Entity(entityType).Metadata);

            var method = entityType.GetInterfaceMap(genericType).InterfaceMethods.First();

            method.Invoke(eInstance, [entityBuilderObject]);
        }
    }

    public virtual DbSet<AuthUser> AuthUsers { get; set; }
    public virtual DbSet<AuthRole> AuthRoles { get; set; }
    public virtual DbSet<Character> Characters { get; set; }
    public virtual DbSet<CharacterStats> CharacterStats { get; set; }
    public virtual DbSet<CharacterIpLog> CharacterIpLogs { get; set; }
    public virtual DbSet<PlayerAccount> PlayerAccounts { get; set; }
    public virtual DbSet<CharacterStatHistory> CharacterStatHistory { get; set; }
    public virtual DbSet<ServerNews> ServerNews { get; set; }
    public virtual DbSet<WebNewsCategory> WebNewsCategories { get; set; }
    public virtual DbSet<WebNewsArticle> WebNewsArticles { get; set; }
    public virtual DbSet<WebNewsReadLog> WebNewsReadLogs { get; set; }
    public virtual DbSet<ChatLobby> ChatLobbies { get; set;}
    public virtual DbSet<AreaServerCategory> AreaServerCategories { get; set;}
    public virtual DbSet<Guild> Guilds { get; set;}
    public virtual DbSet<GuildStats> GuildStats { get; set;}
    public virtual DbSet<BbsCategory> BbsCategories { get; set;}
    public virtual DbSet<BbsPost> BbsPosts { get; set;}
    public virtual DbSet<BbsPostContent> BbsPostContents { get; set;}
    public virtual DbSet<BbsThread> BbsThreads { get; set;}
    public virtual DbSet<AreaServerIpMapping> AreaServerIpMappings { get; set; }
    public virtual DbSet<GuildActivityLog> GuildMembershipLogs { get; set; }
    public virtual DbSet<GuildShopItem> GuildShopItems { get; set; }
    public virtual DbSet<BannedIp> BannedIps { get; set; }
    public virtual DbSet<Mail> Mails { get; set; }
    public virtual DbSet<MailContent> MailContents { get; set; }
}
