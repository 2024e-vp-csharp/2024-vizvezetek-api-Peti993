using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Vizvezetek.API.Models;

public partial class vizvezetekContext : DbContext
{
    public vizvezetekContext()
    {
    }

    public vizvezetekContext(DbContextOptions<vizvezetekContext> options)
        : base(options)
    {
    }

    public virtual DbSet<hely> hely { get; set; }

    public virtual DbSet<munkalap> munkalap { get; set; }

    public virtual DbSet<szerelo> szerelo { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;database=vizvezetek", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<hely>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");
        });

        modelBuilder.Entity<munkalap>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.HasOne(d => d.hely).WithMany(p => p.munkalap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("munkalap_ibfk_1");

            entity.HasOne(d => d.szerelo).WithMany(p => p.munkalap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("munkalap_ibfk_2");
        });

        modelBuilder.Entity<szerelo>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
