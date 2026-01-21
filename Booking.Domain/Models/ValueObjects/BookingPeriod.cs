namespace Booking.Domain.Models.ValueObjects;

public record BookingPeriod
{
    public DateOnly CheckInDate { get; }
    public DateOnly CheckOutDate { get; }
    public int Nights => CheckOutDate.DayNumber - CheckInDate.DayNumber;

    private BookingPeriod(DateOnly checkIn, DateOnly checkOut)
    {
        if (IsValid(checkIn, checkOut))
        {
            CheckInDate = checkIn;
            CheckOutDate = checkOut;
        }
        else
            throw new Exceptions.InvalidBookingPeriodException(
                $"Booking period invalid: Check-in {checkIn}, Check-out {checkOut}. Check-in must be today or later and check-out must be after check-in.");
    }

    private static bool IsValid(DateOnly checkIn, DateOnly checkOut) =>
        checkIn >= DateOnly.FromDateTime(DateTime.Today) && checkOut > checkIn;

    public static bool TryParse(DateOnly checkIn, DateOnly checkOut, out BookingPeriod? result)
    {
        result = null;
        if (!IsValid(checkIn, checkOut)) return false;
        try { result = new(checkIn, checkOut); return true; }
        catch { return false; }
    }

    public override string ToString() => $"{CheckInDate} to {CheckOutDate} ({Nights} nights)";
}
