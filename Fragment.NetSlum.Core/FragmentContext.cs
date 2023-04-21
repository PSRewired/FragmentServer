using Fragment.NetSlum.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Core;

public class FragmentContext : DbContext
{
    public FragmentContext()
    {
    }

    public FragmentContext(DbContextOptions<FragmentContext> options)
        : base(options)
    {
    }



    public virtual DbSet<BbsCategory> BbsCategories { get; set; }

    public virtual DbSet<BbsPostBody> BbsPostBodies { get; set; }

    public virtual DbSet<BbsPostMetum> BbsPostMeta { get; set; }

    public virtual DbSet<BbsThread> BbsThreads { get; set; }

    public virtual DbSet<Characterrepository> Characterrepositories { get; set; }

    public virtual DbSet<FailedJob> FailedJobs { get; set; }

    public virtual DbSet<Guilditemshop> Guilditemshops { get; set; }

    public virtual DbSet<Guildrepository> Guildrepositories { get; set; }

    public virtual DbSet<MailBody> MailBodies { get; set; }

    public virtual DbSet<MailMetum> MailMeta { get; set; }

    public virtual DbSet<Messageoftheday> Messageofthedays { get; set; }

    public virtual DbSet<Migration> Migrations { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsSection> NewsSections { get; set; }

    public virtual DbSet<NewsSectionLog> NewsSectionLogs { get; set; }

    public virtual DbSet<PasswordReset> PasswordResets { get; set; }

    public virtual DbSet<PlayerAccountId> PlayerAccountIds { get; set; }

    public virtual DbSet<RankingDatum> RankingData { get; set; }

    public virtual DbSet<SaveFileIntegration> SaveFileIntegrations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use snake_cased table/column names to keep things consistent with the old database layout
        optionsBuilder
            .UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<BbsCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");
        });

        modelBuilder.Entity<BbsPostBody>(entity =>
        {
            entity.HasKey(e => e.PostBodyId).HasName("PRIMARY");

            entity.HasOne(d => d.Post).WithMany(p => p.BbsPostBodies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BBS_Post_Body_BBS_Post_Meta_postID_fk");
        });

        modelBuilder.Entity<BbsPostMetum>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PRIMARY");

            entity.HasOne(d => d.Thread).WithMany(p => p.BbsPostMeta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BBS_Post_Meta_BBS_Threads_threadID_fk");
        });

        modelBuilder.Entity<BbsThread>(entity =>
        {
            entity.HasKey(e => e.ThreadId).HasName("PRIMARY");

            entity.HasOne(d => d.Category).WithMany(p => p.BbsThreads)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BBS_Threads_BBS_Category_categoryID_fk");
        });

        modelBuilder.Entity<Characterrepository>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PRIMARY");
        });

        modelBuilder.Entity<FailedJob>(entity =>
        {
            entity.Property(e => e.FailedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Guilditemshop>(entity =>
        {
            entity.HasKey(e => e.ItemShopId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Guildrepository>(entity =>
        {
            entity.HasKey(e => e.GuildId).HasName("PRIMARY");
        });

        modelBuilder.Entity<MailBody>(entity =>
        {
            entity.HasKey(e => e.MailBodyId).HasName("PRIMARY");

            entity.HasOne(d => d.Mail).WithOne(p => p.MailBody)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MAIL_BODY_MAIL_META_MAIL_ID_fk");
        });

        modelBuilder.Entity<MailMetum>(entity =>
        {
            entity.HasKey(e => e.MailId).HasName("PRIMARY");

            entity.Property(e => e.MailDelivered).HasDefaultValueSql("'0'");
        });

        modelBuilder.Entity<Messageoftheday>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PRIMARY");

            entity.ToTable("news", tb => tb.HasComment("type:\r\n1 = news\r\n2 = current maint\r\n3 = planned maint\r\n4 = past maint"));

            entity.Property(e => e.Date).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<NewsSection>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("PRIMARY");
        });

        modelBuilder.Entity<NewsSectionLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<PlayerAccountId>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<RankingDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CharacterGp).HasDefaultValueSql("'0'");
            entity.Property(e => e.GodStatueCounterOnline).HasDefaultValueSql("'0'");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Enabled).HasDefaultValueSql("'1'");
        });
    }
}
