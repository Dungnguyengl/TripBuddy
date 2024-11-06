using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ImageService.Models;

public partial class ImageServiceDbContext : DbContext
{
    public ImageServiceDbContext()
    {
    }

    public ImageServiceDbContext(DbContextOptions<ImageServiceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ImageHead> ImageHeads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ImageHead>(entity =>
        {
            entity.HasKey(e => e.ImageKey).HasName("PK__IMAGE_HE__3FCF77419A073578");

            entity.Property(e => e.ImageKey).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
