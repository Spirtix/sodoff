﻿using Microsoft.EntityFrameworkCore;

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
    public DbSet<InventoryItem> InventoryItems { get; set; } = null!;
    public DbSet<MissionState> MissionStates { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<RoomItem> RoomItems { get; set; } = null!;
    public DbSet<GameData> GameData { get; set; } = null;
    public DbSet<GameDataPair> GameDataPairs { get; set; } = null;

    public string DbPath { get; }

    public DBContext() {
        DbPath = Path.Join(Directory.GetCurrentDirectory(), "sodoff.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}").UseLazyLoadingProxies();

    protected override void OnModelCreating(ModelBuilder builder) {
        // Sessions
        builder.Entity<Session>().HasOne(s => s.User)
            .WithMany(e => e.Sessions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Session>().HasOne(s => s.Viking)
            .WithMany(e => e.Sessions)
            .HasForeignKey(e => e.VikingId)
            .OnDelete(DeleteBehavior.Cascade);

        // Users
        builder.Entity<User>().HasMany(u => u.Sessions)
            .WithOne(e => e.User);

        builder.Entity<User>().HasMany(u => u.PairData)
            .WithOne(e => e.User);

        builder.Entity<User>().HasMany(u => u.Vikings)
            .WithOne(e => e.User);

        // Vikings
        builder.Entity<Viking>().HasOne(v => v.User)
            .WithMany(e => e.Vikings)
            .HasForeignKey(e => e.UserId);

        builder.Entity<Viking>().HasMany(v => v.Dragons)
            .WithOne(e => e.Viking);

        builder.Entity<Viking>().HasMany(v => v.Sessions)
            .WithOne(e => e.Viking);

        builder.Entity<Viking>().HasMany(v => v.MissionStates)
            .WithOne(e => e.Viking);

        builder.Entity<Viking>().HasMany(v => v.Rooms)
            .WithOne(e => e.Viking);

        builder.Entity<Viking>().HasMany(v => v.AchievementPoints)
            .WithOne(e => e.Viking);

        builder.Entity<Viking>().HasMany(v => v.PairData)
            .WithOne(e => e.Viking);

        builder.Entity<Viking>().HasMany(v => v.Images)
            .WithOne(e => e.Viking);

        builder.Entity<Viking>().HasMany(v => v.TaskStatuses)
            .WithOne(e => e.Viking);

        builder.Entity<Viking>().HasOne(v => v.SelectedDragon)
            .WithOne()
            .HasForeignKey<Viking>(e => e.SelectedDragonId);

        builder.Entity<Viking>().HasMany(v => v.GameData)
            .WithOne(e => e.Viking);

        // Dragons
        builder.Entity<Dragon>().HasOne(d => d.Viking)
            .WithMany(e => e.Dragons)
            .HasForeignKey(e => e.VikingId);

        builder.Entity<Dragon>().HasMany(d => d.PairData)
            .WithOne(e => e.Dragon);

        // PairData & Pair
        builder.Entity<PairData>().HasMany(p => p.Pairs)
            .WithOne(e => e.PairData);

        builder.Entity<PairData>().HasOne(p => p.Viking)
            .WithMany(e => e.PairData)
            .HasForeignKey(e => e.VikingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PairData>().HasOne(p => p.User)
            .WithMany(e => e.PairData)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PairData>().HasOne(p => p.Dragon)
            .WithMany(e => e.PairData)
            .HasForeignKey(e => e.DragonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Pair>()
            .HasOne(p => p.PairData)
            .WithMany(pd => pd.Pairs)
            .HasForeignKey(p => p.MasterId)
            .HasPrincipalKey(e => e.Id);

        // Inventory & InventoryItem
        builder.Entity<Viking>()
            .HasMany(v => v.InventoryItems)
            .WithOne(i => i.Viking);

        builder.Entity<InventoryItem>()
            .HasOne(e => e.Viking)
            .WithMany(e => e.InventoryItems)
            .HasForeignKey(e => e.VikingId);

        // Room & RoomItem
        builder.Entity<Room>().HasOne(r => r.Viking)
            .WithMany(e => e.Rooms)
            .HasForeignKey(e => e.VikingId);

        builder.Entity<Room>().HasMany(r => r.Items)
            .WithOne(e => e.Room);

        builder.Entity<RoomItem>().HasOne(i => i.Room)
            .WithMany(r => r.Items)
            .HasForeignKey(e => e.RoomId);

        // GameData

        builder.Entity<GameData>().HasOne(e => e.Viking)
            .WithMany(e => e.GameData);

        builder.Entity<GameData>().HasMany(e => e.GameDataPairs)
            .WithOne(e => e.GameData);

        builder.Entity<GameDataPair>().HasOne(e => e.GameData)
            .WithMany(e => e.GameDataPairs);

        // Others ..
        builder.Entity<Image>().HasOne(s => s.Viking)
            .WithMany(e => e.Images)
            .HasForeignKey(e => e.VikingId);

        builder.Entity<TaskStatus>().HasKey(e => new { e.Id, e.VikingId, e.MissionId });

        builder.Entity<TaskStatus>()
            .HasOne(t => t.Viking)
            .WithMany(v => v.TaskStatuses)
            .HasForeignKey(t => t.VikingId);

        builder.Entity<MissionState>().HasOne(m => m.Viking)
            .WithMany(e => e.MissionStates)
            .HasForeignKey(e => e.VikingId);

        builder.Entity<AchievementPoints>().HasKey(e => new { e.VikingId, e.Type });

        builder.Entity<AchievementPoints>()
            .HasOne(e => e.Viking)
            .WithMany(e => e.AchievementPoints)
            .HasForeignKey(e => e.VikingId);
    }
}
