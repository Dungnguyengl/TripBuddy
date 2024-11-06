using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UserService.Models;

public partial class UserServiceDbContext : DbContext
{
    public UserServiceDbContext()
    {
    }

    public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserHead> UserHeads { get; set; }

    public virtual DbSet<UserSpot> UserSpots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserHead>(entity =>
        {
            entity.HasKey(e => e.UserKey).HasName("PK__USER_HEA__5F13FD3C5BF9813E");

            entity.Property(e => e.UserKey).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsActivie).HasDefaultValue(false);
        });

        modelBuilder.Entity<UserSpot>(entity =>
        {
            entity.Property(e => e.NoPlace).HasDefaultValue(0L);
            entity.Property(e => e.NoTrip).HasDefaultValue(0L);
            entity.Property(e => e.Rating).HasDefaultValue((short)0);
            entity.Property(e => e.RewardPoint).HasDefaultValue(0L);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
