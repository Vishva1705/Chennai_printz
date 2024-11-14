using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BillingMVCProject.Models
{
    public partial class billingContext : DbContext
    {
        public billingContext()
        {
        }

        public billingContext(DbContextOptions<billingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CustomersDetail> CustomersDetails { get; set; } = null!;
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; } = null!;
        public virtual DbSet<Invoiceitemdetail> Invoiceitemdetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;initial catalog=billing;persist security info=False;user=root;password=Mysql@123;connection timeout=30", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<CustomersDetail>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PRIMARY");

                entity.ToTable("customers_details");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_id");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(100)
                    .HasColumnName("Customer_Name")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(100)
                    .HasColumnName("Email_id")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.GstNumber)
                    .HasMaxLength(50)
                    .HasColumnName("GST_Number");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(100)
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.HasKey(e => e.InvoiceId)
                    .HasName("PRIMARY");

                entity.ToTable("invoice_details");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DeliveryCharge).HasPrecision(10, 2);

                entity.Property(e => e.GrandTotal).HasPrecision(10, 2);

                entity.Property(e => e.GstAmount).HasPrecision(10, 2);

                entity.Property(e => e.Notes).HasColumnType("text");
            });

            modelBuilder.Entity<Invoiceitemdetail>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("PRIMARY");

                entity.ToTable("invoiceitemdetails");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Gsm).HasMaxLength(50);

                entity.Property(e => e.InvoiceId).HasColumnType("text");

                entity.Property(e => e.Side).HasMaxLength(50);

                entity.Property(e => e.Size).HasMaxLength(50);

                entity.Property(e => e.Total)
                    .HasPrecision(10, 2)
                    .HasComputedColumnSql("`Quantity` * `UnitPrice`", true);

                entity.Property(e => e.TotalValues).HasPrecision(18, 2);

                entity.Property(e => e.UnitPrice).HasPrecision(10, 2);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
