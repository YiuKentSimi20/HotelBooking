using Booking.Domain.Models.ValueObjects;
using Booking.Domain.Repositories;
using static Booking.Domain.Models.Entities.TicketBooking;

namespace Booking.Domain.Operations;

internal sealed class ValidateBookingOperation : BookingOperation
{
    private readonly IRoomRepository _roomRepository;

    public ValidateBookingOperation(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    protected override IBooking OnUnvalidated(UnvalidatedBooking booking)
    {
        var errors = new List<string>();
        var cmd = booking.Command;

        if (!CustomerName.TryParse(cmd.CustomerName, out var name))
            errors.Add($"Invalid customer name: {cmd.CustomerName}");

        if (!CustomerEmail.TryParse(cmd.CustomerEmail, out var email))
            errors.Add($"Invalid customer email: {cmd.CustomerEmail}");

        if (!RoomType.TryParse(cmd.RoomType, out var roomType))
            errors.Add($"Invalid room type: {cmd.RoomType}");

        if (!DateOnly.TryParse(cmd.CheckInDate, out var checkInDate))
            errors.Add($"Invalid check-in date: {cmd.CheckInDate}");

        if (!DateOnly.TryParse(cmd.CheckOutDate, out var checkOutDate))
            errors.Add($"Invalid check-out date: {cmd.CheckOutDate}");

        if (errors.Any())
            return new InvalidBooking(errors);

        // Validate date range
        if (checkInDate < DateOnly.FromDateTime(DateTime.Today))
            return new InvalidBooking(new[] { $"Check-in date must be today or later: {checkInDate}" });

        if (checkOutDate <= checkInDate)
            return new InvalidBooking(new[] { $"Check-out date must be after check-in date: {checkInDate} to {checkOutDate}" });

        // Check room availability
        var isAvailable = _roomRepository.IsRoomAvailableAsync(roomType!, checkInDate, checkOutDate).GetAwaiter().GetResult();
        if (!isAvailable)
            return new InvalidBooking(new[] { $"No rooms of type '{roomType!.Value}' available for the period {checkInDate} to {checkOutDate}" });

        return new ValidatedBooking(name!, email!, roomType!, checkInDate, checkOutDate);
    }
}
