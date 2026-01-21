using System.Text.RegularExpressions;

namespace Booking.Domain.Models.ValueObjects;

public record CustomerEmail
{
    private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    public string Value { get; }

    private CustomerEmail(string value)
    {
        if (IsValid(value))
            Value = value;
        else
            throw new Exceptions.InvalidCustomerEmailException($"Email '{value}' is invalid.");
    }

    private static bool IsValid(string value) =>
        !string.IsNullOrWhiteSpace(value) && EmailPattern.IsMatch(value);

    public static bool TryParse(string input, out CustomerEmail? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(input)) return false;
        if (!IsValid(input)) return false;
        try { result = new(input); return true; }
        catch { return false; }
    }

    public override string ToString() => Value;
}
