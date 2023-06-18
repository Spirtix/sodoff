using Microsoft.EntityFrameworkCore;

namespace sodoff.Model;
public class DBContext : DbContext {
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Viking> Vikings { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
    public string DbPath { get; }

    public DBContext() {
        DbPath = Path.Join(Directory.GetCurrentDirectory(), "sodoff.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}").UseLazyLoadingProxies();

    protected override void OnModelCreating(ModelBuilder builder) {
        builder.Entity<Session>().HasOne(s => s.User)
            .WithMany(e => e.Sessions)
            .HasForeignKey(e => e.UserId);

        builder.Entity<Session>().HasOne(s => s.Viking)
            .WithMany(e => e.Sessions)
            .HasForeignKey(e => e.VikingId);

        builder.Entity<User>().HasMany(u => u.Sessions)
            .WithOne(e => e.User);

        builder.Entity<Viking>().HasMany(u => u.Sessions)
            .WithOne(e => e.Viking);

    }
}
