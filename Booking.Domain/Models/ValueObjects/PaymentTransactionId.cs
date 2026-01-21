using System.Text.RegularExpressions;

namespace Booking.Domain.Models.ValueObjects;

public record PaymentTransactionId
{
    private static readonly Regex Pattern = new(@"^[A-Z0-9\-]{10,100}$", RegexOptions.Compiled);
    public string Value { get; }

    private PaymentTransactionId(string value)
    {
        if (IsValid(value))
            Value = value;
        else
            throw new Exceptions.InvalidPaymentTransactionIdException($"Payment transaction ID '{value}' is invalid.");
    }

    private static bool IsValid(string value) =>
        !string.IsNullOrWhiteSpace(value) && Pattern.IsMatch(value);

    public static bool TryParse(string input, out PaymentTransactionId? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(input)) return false;
        if (!IsValid(input)) return false;
        try { result = new(input); return true; }
        catch { return false; }
    }

    public override string ToString() => Value;
}
