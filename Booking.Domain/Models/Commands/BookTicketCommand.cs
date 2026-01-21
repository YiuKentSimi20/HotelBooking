namespace Booking.Domain.Models.Commands;

public record BookTicketCommand(
    string CustomerName,
    string CustomerEmail,
    string RoomType,
    string CheckInDate,
    string CheckOutDate
);
