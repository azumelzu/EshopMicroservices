namespace Ordering.Domain.ValueObjects;

public record ProductId : IValue<Guid>
{
    public Guid Value { get;  }
    
    private ProductId(Guid value) => Value = value;
    
    public static ProductId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Equals(Guid.Empty))
        {
            throw new DomainException($"{nameof(ProductId)} cannot be empty.");
        }

        return new ProductId(value);
    }
};