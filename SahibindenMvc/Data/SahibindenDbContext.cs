using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SahibindenMvc.Models;

namespace SahibindenMvc.Data;

public partial class SahibindenDbContext : DbContext
{
    public SahibindenDbContext()
    {
    }

    public SahibindenDbContext(DbContextOptions<SahibindenDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AttributeDefinition> AttributeDefinitions { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Listing> Listings { get; set; }

    public virtual DbSet<ListingAttribute> ListingAttributes { get; set; }

    public virtual DbSet<ListingPhoto> ListingPhotos { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VwListing> VwListings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:SahibindenDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AttributeDefinition>(entity =>
        {
            entity.HasKey(e => e.AttributeDefId).HasName("PK__Attribut__25E78980B42B332F");

            entity.HasOne(d => d.Category).WithMany(p => p.AttributeDefinitions).HasConstraintName("FK_AttrDefs_Categories");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B356BFF74");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_Categories_Parent");
        });

        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasKey(e => e.ListingId).HasName("PK__Listings__BF3EBED024DDE0EF");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Currency)
                .HasDefaultValue("TRY")
                .IsFixedLength();
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Category).WithMany(p => p.Listings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Listings_Categories");

            entity.HasOne(d => d.Location).WithMany(p => p.Listings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Listings_Locations");

            entity.HasOne(d => d.User).WithMany(p => p.Listings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Listings_Users");
        });

        modelBuilder.Entity<ListingAttribute>(entity =>
        {
            entity.HasKey(e => e.ListingAttrId).HasName("PK__ListingA__5ABF5EB8B8754238");

            entity.HasOne(d => d.AttributeDef).WithMany(p => p.ListingAttributes).HasConstraintName("FK_ListingAttrs_AttrDefs");

            entity.HasOne(d => d.Listing).WithMany(p => p.ListingAttributes).HasConstraintName("FK_ListingAttrs_Listings");
        });

        modelBuilder.Entity<ListingPhoto>(entity =>
        {
            entity.HasKey(e => e.PhotoId).HasName("PK__ListingP__21B7B5E2D8905420");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Listing).WithMany(p => p.ListingPhotos).HasConstraintName("FK_ListingPhotos_Listings");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA49707C1F1DC");

            entity.HasIndex(e => e.LocationName, "UX_Locations_City")
                .IsUnique()
                .HasFilter("([LocationType]=(1))");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_Locations_Parent");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CE928BE6C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
        });

        modelBuilder.Entity<VwListing>(entity =>
        {
            entity.ToView("vw_Listings");

            entity.Property(e => e.Currency).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
