using FrontDesk.Domain.Models.ValueObjects;
using FrontDesk.Domain.Repositories;
using static FrontDesk.Domain.Models.Entities.CheckIn;

namespace FrontDesk.Domain.Operations;

internal sealed class ValidateCheckInOperation : CheckInOperation
{
    private readonly IRoomRepository _roomRepository;

    internal ValidateCheckInOperation(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    protected override ICheckIn OnUnvalidated(UnvalidatedCheckIn checkIn)
    {
        var errors = new List<string>();
        var cmd = checkIn.Command;

        if (cmd.BookingId <= 0)
            errors.Add($"Invalid booking ID: {cmd.BookingId}");

        if (!CustomerName.TryParse(cmd.CustomerName, out var customerName))
            errors.Add($"Invalid customer name: {cmd.CustomerName}");

        if (!CustomerEmail.TryParse(cmd.CustomerEmail, out var customerEmail))
            errors.Add($"Invalid customer email: {cmd.CustomerEmail}");

        if (string.IsNullOrWhiteSpace(cmd.RoomType))
            errors.Add($"Invalid room type: {cmd.RoomType}");

        if (!RoomNumber.TryParse(cmd.RoomNumber, out var roomNumber))
            errors.Add($"Invalid room number: {cmd.RoomNumber}");

        if (errors.Any())
            return new InvalidCheckIn(errors);

        // Get room ID from room number
        var roomId = _roomRepository.GetRoomIdByNumberAsync(roomNumber!.Value).GetAwaiter().GetResult();
        if (!roomId.HasValue)
            return new InvalidCheckIn(new[] { $"Room not found: {cmd.RoomNumber}" });

        // Check if room is available (not out of service)
        var isAvailable = _roomRepository.IsRoomAvailableAsync(roomId.Value).GetAwaiter().GetResult();
        if (!isAvailable)
            return new InvalidCheckIn(new[] { $"Room {cmd.RoomNumber} is not available (out of service)" });

        return new ValidatedCheckIn(
            cmd.BookingId,
            customerName!,
            customerEmail!,
            cmd.RoomType,
            roomNumber!,
            cmd.CheckInDate,
            cmd.CheckOutDate,
            roomId.Value);
    }
}
