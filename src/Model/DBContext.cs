using Microsoft.EntityFrameworkCore;

namespace sodoff.Model;
public class DBContext : DbContext {
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Viking> Vikings { get; set; } = null!;
    public DbSet<Dragon> Dragons { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
    public DbSet<Pair> Pairs { get; set; } = null!;
    public DbSet<PairData> PairData { get; set; } = null!;
    public DbSet<TaskStatus> TaskStatuses { get; set; } = null!;
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

        builder.Entity<Viking>().HasOne(s => s.User)
            .WithMany(e => e.Vikings)
            .HasForeignKey(e => e.UserId);

        builder.Entity<User>().HasMany(u => u.Vikings)
            .WithOne(e => e.User);

        builder.Entity<Dragon>().HasOne(s => s.Viking)
            .WithMany(e => e.Dragons)
            .HasForeignKey(e => e.VikingId);

        builder.Entity<Viking>().HasMany(u => u.Dragons)
            .WithOne(e => e.Viking);

        builder.Entity<Viking>().HasOne(s => s.SelectedDragon)
            .WithOne(e => e.SelectedViking)
            .HasForeignKey<Dragon>(e => e.SelectedVikingId);

        builder.Entity<Dragon>().HasOne(s => s.SelectedViking)
            .WithOne(e => e.SelectedDragon)
            .HasForeignKey<Viking>(e => e.SelectedDragonId);

        builder.Entity<Image>().HasOne(s => s.Viking)
            .WithMany(e => e.Images)
            .HasForeignKey(e => e.VikingId);

        builder.Entity<Viking>().HasMany(u => u.Images)
            .WithOne(e => e.Viking);

        builder.Entity<PairData>()
            .HasKey(e => e.Id);

        builder.Entity<PairData>().HasMany(p => p.Pairs)
            .WithOne(e => e.PairData);

        builder.Entity<Pair>()
            .HasOne(p => p.PairData)
            .WithMany(pd => pd.Pairs)
            .HasForeignKey(p => p.MasterId)
            .HasPrincipalKey(e => e.Id);

        builder.Entity<TaskStatus>().HasKey(e => new { e.Id, e.VikingId, e.MissionId });

        builder.Entity<TaskStatus>()
            .HasOne(t => t.Viking)
            .WithMany();
    }
}
