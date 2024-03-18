using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StationeryCompany.Model;

public partial class StationeryCompanyContext : DbContext
{
    public StationeryCompanyContext()
    {
    }

    public StationeryCompanyContext(DbContextOptions<StationeryCompanyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CustomerCompany> CustomerCompanies { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SalesManager> SalesManagers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=6E7PN2T;Database=StationeryCompany;Integrated Security=SSPI;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerCompany>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Customer__2D971C4C5E68AF17");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED46133342");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Type).WithMany(p => p.Products)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Products__TypeID__267ABA7A");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__ProductT__516F0395876C721F");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.TypeName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__Sales__1EE3C41F751EF10B");

            entity.Property(e => e.SaleId).HasColumnName("SaleID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.PricePerUnit).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Company).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__Sales__CompanyID__2F10007B");

            entity.HasOne(d => d.Manager).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Sales__ManagerID__2E1BDC42");

            entity.HasOne(d => d.Product).WithMany(p => p.Sales)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Sales__ProductID__2D27B809");
        });

        modelBuilder.Entity<SalesManager>(entity =>
        {
            entity.HasKey(e => e.ManagerId).HasName("PK__SalesMan__3BA2AA81097E1DF6");

            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.ManagerName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
