using Invoicing.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Data;

public class InvoicingDbContext : DbContext
{
    public InvoicingDbContext(DbContextOptions<InvoicingDbContext> options) : base(options) { }

    public DbSet<InvoiceEntity> Invoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InvoiceEntity>(entity =>
        {
            entity.ToTable("Invoices");
            entity.HasKey(e => e.InvoiceId);
            entity.Property(e => e.InvoiceId).ValueGeneratedOnAdd();
            entity.Property(e => e.BookingId).IsRequired();
            entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CustomerName).HasMaxLength(200);
            entity.Property(e => e.CustomerEmail).HasMaxLength(200);
            entity.Property(e => e.RoomType).HasMaxLength(50);
            entity.Property(e => e.RoomNumber).HasMaxLength(10);
            entity.Property(e => e.CheckInDate).IsRequired();
            entity.Property(e => e.CheckOutDate).IsRequired();
            entity.Property(e => e.NetValue).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.VatValue).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalValue).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.PaymentTransactionId).HasMaxLength(100);
            entity.Property(e => e.IssuedDate).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.HasIndex(e => e.InvoiceNumber).IsUnique().HasDatabaseName("IX_Invoices_InvoiceNumber");
            entity.HasIndex(e => e.BookingId).IsUnique().HasDatabaseName("IX_Invoices_BookingId");
        });
    }
}


