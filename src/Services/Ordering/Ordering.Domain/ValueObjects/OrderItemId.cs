namespace Ordering.Domain.ValueObjects;

public record OrderItemId : IValue<Guid>
{
    public Guid Value { get;  }
    
    private OrderItemId(Guid value) => Value = value;
    
    public static OrderItemId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Equals(Guid.Empty))
        {
            throw new DomainException($"{nameof(OrderItemId)} cannot be empty.");
        }

        return new OrderItemId(value);
    }
};