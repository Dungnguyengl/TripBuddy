using Microsoft.EntityFrameworkCore;

namespace SpotService.Model;

public partial class SpotDbContext : DbContext
{
    public SpotDbContext()
    {
    }

    public SpotDbContext(DbContextOptions<SpotDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AtrContent> AtrContents { get; set; }

    public virtual DbSet<AtrHead> AtrHeads { get; set; }

    public virtual DbSet<Constant> Constants { get; set; }

    public virtual DbSet<DesHead> DesHeads { get; set; }

    public virtual DbSet<PlcHead> PlcHeads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AtrContent>(entity =>
        {
            entity.HasKey(e => e.ContentKey).HasName("PK__ATR_CONT__9D7F39712E461DE0");

            entity.Property(e => e.ContentKey).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.AtrKeyNavigation).WithMany(p => p.AtrContents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ATR_CONTE__ATR_K__6477ECF3");

            entity.HasOne(d => d.DesKeyNavigation).WithMany(p => p.AtrContents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ATR_CONTE__DES_K__656C112C");
        });

        modelBuilder.Entity<AtrHead>(entity =>
        {
            entity.HasKey(e => e.AtrKey).HasName("PK__ATR_HEAD__4F7460C0AA3C54F3");

            entity.Property(e => e.AtrKey).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<DesHead>(entity =>
        {
            entity.HasKey(e => e.DesKey).HasName("PK__DES_HEAD__3BD1FD27655EEF93");

            entity.Property(e => e.DesKey).HasDefaultValueSql("(newid())");
            entity.Property(e => e.NoVisted).HasDefaultValue(0);

            entity.HasOne(d => d.AtrKeyNavigation).WithMany(p => p.DesHeads)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DES_HEAD__ATR_KE__619B8048");
        });

        modelBuilder.Entity<PlcHead>(entity =>
        {
            entity.HasKey(e => e.PlcKey).HasName("PK__PLC_HEAD__6D4D82FC582F7520");

            entity.Property(e => e.PlcKey).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.AtrKeyNavigation).WithMany(p => p.PlcHeads)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PLC_HEAD__ATR_KE__628FA481");

            entity.HasOne(d => d.DesKeyNavigation).WithMany(p => p.PlcHeads)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PLC_HEAD__DES_KE__6383C8BA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
