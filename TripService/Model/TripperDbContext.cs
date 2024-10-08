using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TripperService.Model;

public partial class TripperDbContext : DbContext
{
    public TripperDbContext()
    {
    }

    public TripperDbContext(DbContextOptions<TripperDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TripHead> TripHeads { get; set; }

    public virtual DbSet<TripPlc> TripPlcs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TripHead>(entity =>
        {
            entity.HasKey(e => e.TripKey).HasName("PK__TRIP_HEA__B0151DB68D9D961E");

            entity.Property(e => e.TripKey).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TripPlc>(entity =>
        {
            entity.HasOne(d => d.TripKeyNavigation).WithMany().HasConstraintName("FK__TRIP_PLC__TRIP_K__68487DD7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
