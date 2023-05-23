using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PROG_POE_PART_2.Model;

public partial class ProgPoeContext : DbContext
{
    public ProgPoeContext()
    {
    }

    public ProgPoeContext(DbContextOptions<ProgPoeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Farmer> Farmers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:prog-poe-server.database.windows.net,1433;Initial Catalog=PROG_POE;Persist Security Info=False;User ID=dylan;Password=Frenchie12!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__C134C9A150F41925");

            entity.Property(e => e.EmployeeId).HasColumnName("employeeID");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("employeeName");
            entity.Property(e => e.EmployeePassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("employeePassword");
            entity.Property(e => e.EmployeeSurname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("employeeSurname");
            entity.Property(e => e.EmployeeUsername)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("employeeUsername");
        });

        modelBuilder.Entity<Farmer>(entity =>
        {
            entity.HasKey(e => e.FarmerId).HasName("PK__Farmers__EC6F88C8D717E6F0");

            entity.Property(e => e.FarmerId).HasColumnName("farmerID");
            entity.Property(e => e.FarmName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("farmName");
            entity.Property(e => e.FarmerName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("farmerName");
            entity.Property(e => e.FarmerPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("farmerPassword");
            entity.Property(e => e.FarmerSurname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("farmerSurname");
            entity.Property(e => e.FarmerUsername)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("farmerUsername");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__2D10D14AE304A90F");

            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.DateAdded)
                .HasColumnType("date")
                .HasColumnName("dateAdded");
            entity.Property(e => e.FarmerId).HasColumnName("farmerID");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("productName");
            entity.Property(e => e.ProductPrice).HasColumnName("productPrice");
            entity.Property(e => e.ProductStock).HasColumnName("productStock");
            entity.Property(e => e.ProductType)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("productType");

            entity.HasOne(d => d.Farmer).WithMany(p => p.Products)
                .HasForeignKey(d => d.FarmerId)
                .HasConstraintName("FK__Products__farmer__5EBF139D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
