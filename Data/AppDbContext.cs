using System;
using System.Collections.Generic;
using EMS_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS_Project.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=LEGION-5-15ACH6\\SQLEXPRESS;Initial Catalog=EMSDB;Encrypt=false;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.HasIndex(e => e.EmployeeId, "EmployeePk").IsUnique();

            entity.Property(e => e.EmployeeId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeEmail)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.EmployeeJob)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.HasKey(e => e.DayId);

            entity.ToTable("Holiday");

            entity.Property(e => e.DayId)
                .ValueGeneratedNever()
                .HasColumnName("DayID");
            entity.Property(e => e.Holiday1).HasColumnName("Holiday");
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
