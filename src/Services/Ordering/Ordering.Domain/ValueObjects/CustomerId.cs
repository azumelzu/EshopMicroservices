namespace Ordering.Domain.ValueObjects;

public record CustomerId : IValue<Guid>
{
    public Guid Value { get; }
    private CustomerId(Guid value) => Value = value;

    public static CustomerId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Equals(Guid.Empty))
        {
            throw new DomainException($"{nameof(CustomerId)} cannot be empty.");
        }

        return new CustomerId(value);
    }
};