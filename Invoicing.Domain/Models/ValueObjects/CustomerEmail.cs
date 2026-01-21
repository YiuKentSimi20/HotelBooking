using System.Text.RegularExpressions;

namespace Invoicing.Domain.Models.ValueObjects;

public record CustomerEmail
{
    public string Value { get; }

    private CustomerEmail(string value)
    {
        Value = value;
    }

    private static bool IsValid(string email) =>
        !string.IsNullOrWhiteSpace(email) &&
        email.Length <= 200 &&
        Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public static bool TryParse(string? value, out CustomerEmail? result)
    {
        result = null;
        if (value is null || !IsValid(value))
            return false;

        result = new CustomerEmail(value);
        return true;
    }

    public override string ToString() => Value;
}
