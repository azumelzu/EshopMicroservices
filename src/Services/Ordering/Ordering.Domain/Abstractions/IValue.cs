namespace Ordering.Domain.Abstractions;

public interface IValue<out T>
{
    T Value { get; }
}