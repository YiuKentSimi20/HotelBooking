namespace Invoicing.Domain.Models.ValueObjects;

public record MoneyValue
{
    public decimal Value { get; }

    private MoneyValue(decimal value)
    {
        Value = value;
    }

    public static bool TryParse(decimal value, out MoneyValue? result)
    {
        result = null;
        if (value < 0)
            return false;

        result = new MoneyValue(Math.Round(value, 2));
        return true;
    }

    public static MoneyValue Zero => new(0);

    public override string ToString() => Value.ToString("0.00");
}
