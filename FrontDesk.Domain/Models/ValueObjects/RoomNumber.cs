namespace FrontDesk.Domain.Models.ValueObjects;

public record RoomNumber
{
    public string Value { get; }

    private RoomNumber(string value)
    {
        Value = value;
    }

    public static bool TryParse(string? value, out RoomNumber? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value) || value.Length > 10)
            return false;

        result = new RoomNumber(value);
        return true;
    }

    public override string ToString() => Value;
}
