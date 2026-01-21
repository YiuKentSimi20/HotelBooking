using Booking.Data.Models;
using Booking.Domain.Repositories;
using static Booking.Domain.Models.Entities.TicketBooking;

namespace Booking.Data.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _context;

    public BookingRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveBookingAsync(PaymentProcessedBooking booking, string roomNumber)
    {
        var entity = new BookingEntity
        {
            CustomerName = booking.CustomerName.Value,
            CustomerEmail = booking.CustomerEmail.Value,
            RoomType = booking.RoomType.Value,
            RoomNumber = roomNumber,
            CheckInDate = booking.CheckInDate.ToDateTime(TimeOnly.MinValue),
            CheckOutDate = booking.CheckOutDate.ToDateTime(TimeOnly.MinValue),
            TotalAmount = booking.TotalAmount.Value,
            PaymentTransactionId = booking.PaymentTransactionId.Value,
            CreatedAt = DateTime.Now
        };

        _context.Bookings.Add(entity);
        await _context.SaveChangesAsync();

        return entity.BookingId;
    }

    public async Task AssignRoomToBookingAsync(int bookingId, int roomId, DateOnly checkInDate, DateOnly checkOutDate)
    {
        var assignment = new RoomAssignmentEntity
        {
            BookingId = bookingId,
            RoomId = roomId,
            CheckInDate = checkInDate.ToDateTime(TimeOnly.MinValue),
            CheckOutDate = checkOutDate.ToDateTime(TimeOnly.MinValue)
        };

        _context.RoomAssignments.Add(assignment);
        await _context.SaveChangesAsync();
    }
}
