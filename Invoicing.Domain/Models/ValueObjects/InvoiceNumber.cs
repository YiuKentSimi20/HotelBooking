namespace Invoicing.Domain.Models.ValueObjects;

public record InvoiceNumber
{
    public string Value { get; }

    private InvoiceNumber(string value)
    {
        Value = value;
    }

    public static InvoiceNumber Generate(int bookingId)
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        return new InvoiceNumber($"INV-{bookingId}-{timestamp}");
    }

    public static bool TryParse(string value, out InvoiceNumber? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
            return false;

        result = new InvoiceNumber(value);
        return true;
    }

    public override string ToString() => Value;
}
