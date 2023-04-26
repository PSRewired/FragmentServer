using Microsoft.EntityFrameworkCore;

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
    }
}
