namespace Booking.Domain.Models.ValueObjects;

public record  RoomType
{
    private static readonly HashSet<string> ValidTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Single", "Double", "Suite"
    };

    public string Value { get; }

    private RoomType(string value)
    {
        if (IsValid(value))
            Value = value;
        else
            throw new Exceptions.InvalidRoomTypeException($"Room type '{value}' is invalid. Must be: Single, Double, or Suite.");
    }

    private static bool IsValid(string value) =>
        !string.IsNullOrWhiteSpace(value) && ValidTypes.Contains(value);

    public static bool TryParse(string input, out RoomType? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(input)) return false;
        if (!IsValid(input)) return false;
        try { result = new(input); return true; }
        catch { return false; }
    }

    public override string ToString() => Value;
}
