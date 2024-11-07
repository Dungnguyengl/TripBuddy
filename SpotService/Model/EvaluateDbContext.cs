using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SpotService.Model;

public partial class EvaluateDbContext : DbContext
{
    public EvaluateDbContext()
    {
    }

    public EvaluateDbContext(DbContextOptions<EvaluateDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EvlHead> EvlHeads { get; set; }

    public virtual DbSet<EvlPlc> EvlPlcs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EvlHead>(entity =>
        {
            entity.HasKey(e => e.EvlKey).HasName("PK__EVL_HEAD__4529C5FC3F30D7DF");

            entity.ToTable("EVL_HEAD", "EVALUATE", tb => tb.HasTrigger("AverageOnInsert"));

            entity.Property(e => e.EvlKey).ValueGeneratedNever();
        });

        modelBuilder.Entity<EvlPlc>(entity =>
        {
            entity.HasKey(e => new { e.AtrKey, e.DesKey, e.PlcKey }).HasName("PK__EVL_PLC__D1A432907F1A8A6E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
