using Booking.Domain.Repositories;
using static Booking.Domain.Models.Entities.TicketBooking;

namespace Booking.Domain.Operations;

internal sealed class AssignRoomOperation : BookingOperation
{
    private readonly IRoomRepository _roomRepository;

    internal AssignRoomOperation(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    protected override IBooking OnValidated(ValidatedBooking booking)
    {
        // Get an available room for the booking
        var availableRoom = _roomRepository
            .GetAvailableRoomAsync(booking.RoomType, booking.CheckInDate, booking.CheckOutDate)
            .GetAwaiter()
            .GetResult();

        if (!availableRoom.HasValue)
        {
            return new InvalidBooking(new[] 
            { 
                $"No rooms of type '{booking.RoomType.Value}' available for the period {booking.CheckInDate} to {booking.CheckOutDate}" 
            });
        }

        var (roomId, roomNumber, pricePerNight) = availableRoom.Value;

        return new RoomAssignedBooking(
            booking.CustomerName,
            booking.CustomerEmail,
            booking.RoomType,
            booking.CheckInDate,
            booking.CheckOutDate,
            roomId,
            roomNumber,
            pricePerNight);
    }
}
