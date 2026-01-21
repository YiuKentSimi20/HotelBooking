namespace Invoicing.Domain.Models.ValueObjects;

public record CustomerName
{
    public string Value { get; }

    private CustomerName(string value)
    {
        Value = value;
    }

    public static bool TryParse(string? value, out CustomerName? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 200)
            return false;

        result = new CustomerName(value);
        return true;
    }

    public override string ToString() => Value;
}
