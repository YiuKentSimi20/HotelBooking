namespace FrontDesk.Domain.Models.ValueObjects;

public record AccessCode
{
    public string Value { get; }

    private AccessCode(string value)
    {
        Value = value;
    }

    public static AccessCode Generate()
    {
        // Generate a 6-digit access code
        var random = new Random();
        var code = random.Next(100000, 999999).ToString();
        return new AccessCode(code);
    }

    public static bool TryParse(string value, out AccessCode? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(value) || value.Length > 20)
            return false;

        result = new AccessCode(value);
        return true;
    }

    public override string ToString() => Value;
}
