using static Booking.Domain.Models.Entities.TicketBooking;

namespace Booking.Domain.Repositories;

public interface IBookingRepository
{
    Task<int> SaveBookingAsync(PaymentProcessedBooking booking, string roomNumber);
    Task AssignRoomToBookingAsync(int bookingId, int roomId, DateOnly checkInDate, DateOnly checkOutDate);
}
