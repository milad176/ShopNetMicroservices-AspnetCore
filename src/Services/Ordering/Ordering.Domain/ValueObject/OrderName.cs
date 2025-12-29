namespace Ordering.Domain.ValueObject;

public record OrderName
{
    private const int DefaultLength = 2;
    public string Value { get; } = default!;
    private OrderName(string value) => Value = value;

    public static OrderName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultLength);

        return new OrderName(value);
    }
}