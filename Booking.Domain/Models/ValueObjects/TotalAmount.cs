namespace Booking.Domain.Models.ValueObjects;

public record TotalAmount
{
    public decimal Value { get; }

    private TotalAmount(decimal value)
    {
        if (IsValid(value))
            Value = value;
        else
            throw new Exceptions.InvalidTotalAmountException($"Total amount {value} is invalid. Must be positive.");
    }

    private static bool IsValid(decimal value) => value > 0;

    public static bool TryParse(decimal value, out TotalAmount? result)
    {
        result = null;
        if (!IsValid(value)) return false;
        try { result = new(value); return true; }
        catch { return false; }
    }

    public override string ToString() => Value.ToString("F2");
}
