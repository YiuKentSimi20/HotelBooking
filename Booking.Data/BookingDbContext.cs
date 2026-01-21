using Booking.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

    public DbSet<BookingEntity> Bookings { get; set; }
    public DbSet<RoomEntity> Rooms { get; set; }
    public DbSet<RoomAssignmentEntity> RoomAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingEntity>(entity =>
        {
            entity.ToTable("Bookings");
            entity.HasKey(e => e.BookingId);
            entity.Property(e => e.BookingId).ValueGeneratedOnAdd();
            entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CustomerEmail).IsRequired().HasMaxLength(200);
            entity.Property(e => e.RoomType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.RoomNumber).IsRequired().HasMaxLength(10);
            entity.Property(e => e.CheckInDate).IsRequired();
            entity.Property(e => e.CheckOutDate).IsRequired();
            entity.Property(e => e.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.PaymentTransactionId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.HasIndex(e => e.CustomerEmail).HasDatabaseName("IX_Bookings_CustomerEmail");
        });

        modelBuilder.Entity<RoomEntity>(entity =>
        {
            entity.ToTable("Rooms");
            entity.HasKey(e => e.RoomId);
            entity.Property(e => e.RoomId).ValueGeneratedOnAdd();
            entity.Property(e => e.RoomNumber).IsRequired().HasMaxLength(10);
            entity.Property(e => e.RoomType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsClean).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.IsOutOfService).IsRequired().HasDefaultValue(false);
            entity.Property(e => e.PricePerNight).IsRequired().HasColumnType("decimal(18,2)");
            entity.HasIndex(e => e.RoomNumber).IsUnique().HasDatabaseName("IX_Rooms_RoomNumber");
        });

        modelBuilder.Entity<RoomAssignmentEntity>(entity =>
        {
            entity.ToTable("RoomAssignments");
            entity.HasKey(e => e.AssignmentId);
            entity.Property(e => e.AssignmentId).ValueGeneratedOnAdd();
            entity.Property(e => e.BookingId).IsRequired();
            entity.Property(e => e.RoomId).IsRequired();
            entity.Property(e => e.CheckInDate).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.CheckOutDate).IsRequired(false);
            entity.HasIndex(e => e.RoomId).HasDatabaseName("IX_RoomAssignments_RoomId");
        });
    }
}
