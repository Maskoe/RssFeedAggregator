using Microsoft.EntityFrameworkCore;

namespace MockStuff.Db;

public class AppDbContext : DbContext
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Feed> Feeds { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>()
            .HasMany(x => x.Feeds)
            .WithMany(x => x.AppUsers)
            .UsingEntity<Subscription>();

        modelBuilder.Entity<Subscription>()
            .HasIndex(x => new { x.FeedId, x.AppUserId })
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}