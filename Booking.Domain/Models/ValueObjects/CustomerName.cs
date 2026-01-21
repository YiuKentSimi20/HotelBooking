namespace Booking.Domain.Models.ValueObjects;

public record CustomerName
{
    public string Value { get; }

    private CustomerName(string value)
    {
        if (IsValid(value))
            Value = value;
        else
            throw new Exceptions.InvalidCustomerNameException($"Customer name '{value}' is invalid. Must be 2-200 characters.");
    }

    private static bool IsValid(string value) =>
        !string.IsNullOrWhiteSpace(value) && value.Length >= 2 && value.Length <= 200;

    public static bool TryParse(string input, out CustomerName? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(input)) return false;
        if (!IsValid(input)) return false;
        try { result = new(input); return true; }
        catch { return false; }
    }

    public override string ToString() => Value;
}
