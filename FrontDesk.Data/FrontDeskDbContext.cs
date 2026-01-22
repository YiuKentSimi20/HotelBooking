using FrontDesk.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontDesk.Data;

public class FrontDeskDbContext : DbContext
{
    public FrontDeskDbContext(DbContextOptions<FrontDeskDbContext> options) : base(options) { }

    public DbSet<RoomEntity> Rooms { get; set; }
    public DbSet<RoomAssignmentEntity> RoomAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoomEntity>(entity =>
        {
            entity.ToTable("Rooms");
            entity.HasKey(e => e.RoomId);
            entity.Property(e => e.RoomId).ValueGeneratedOnAdd();
            entity.Property(e => e.RoomNumber).IsRequired().HasMaxLength(10);
            entity.Property(e => e.RoomType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsClean).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.IsOutOfService).IsRequired().HasDefaultValue(false);
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
            entity.Property(e => e.AccessCode).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.RoomId).HasDatabaseName("IX_RoomAssignments_RoomId");
        });
    }
}
