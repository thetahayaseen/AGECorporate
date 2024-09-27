using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AGECorporate.Models;

public partial class AgeCorporateContext : IdentityDbContext
{
    public AgeCorporateContext()
    {
    }

    public AgeCorporateContext(DbContextOptions<AgeCorporateContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Query> Queries { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<TimelineEntry> TimelineEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-KMKAHQA\\SQLEXPRESS01; Database=AgeCorporate; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blogs__3214EC079E324A44");

            entity.Property(e => e.Header).HasMaxLength(255);
        });

        modelBuilder.Entity<Query>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Queries__3214EC0748BD5A16");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Query1).HasColumnName("Query");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Testimon__3214EC079887073C");

            entity.Property(e => e.Email).HasMaxLength(450);
            entity.Property(e => e.Testimonial1).HasColumnName("Testimonial");
        });

        modelBuilder.Entity<TimelineEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Timeline__3214EC0713CDB7BA");

            entity.Property(e => e.Header).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
