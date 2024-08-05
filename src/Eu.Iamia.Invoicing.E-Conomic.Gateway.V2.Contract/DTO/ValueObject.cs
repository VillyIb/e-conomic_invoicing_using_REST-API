namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO;

public abstract class ValueObject<T> where T : ValueObject<T>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();


    public override bool Equals(object? obj) =>
        obj is T valueObject &&
        valueObject.GetType() == GetType() &&
        GetEqualityComponents()
            .SequenceEqual(valueObject.GetEqualityComponents());


    public override int GetHashCode() => GetEqualityComponents()
        .Aggregate(
            1,
            (current, aggregate) => current * 23 + (aggregate?.GetHashCode() ?? 0
                )
        );


    // Operator overloading for ==
#pragma warning disable S3875 // All subclasses are intended to have value semantics and thus can be compared by == operator.
    public static bool operator ==(ValueObject<T>? a, ValueObject<T>? b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;

        return a.Equals(b);
    }

    // Operator overloading for !=
    public static bool operator !=(ValueObject<T>? a, ValueObject<T>? b)
    {
        return !(a == b);
    }
}
