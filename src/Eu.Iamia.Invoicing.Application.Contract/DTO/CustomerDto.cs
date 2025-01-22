namespace Eu.Iamia.Invoicing.Application.Contract.DTO;

public class CustomerDto : ValueObject<CustomerDto>, Reporting.Contract.ICustomer
{
    public int CustomerNumber { get; init; }

    public int CustomerGroupNumber { get; init; }

    public int PaymentTerms { get; init; }

    public string? Address { get; init; }

    public string? City { get; init; }

    public string? Name { get; init; }

    public string? Zip { get; init; }

    public bool IsBarred { get; init; }

    public override string ToString()
    {
        return $"{nameof(CustomerNumber)}: {CustomerNumber}, {nameof(Name)}: {Name}, {nameof(CustomerGroupNumber)}: {CustomerGroupNumber}";
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CustomerNumber;
        yield return CustomerGroupNumber;
        yield return PaymentTerms;
        yield return Address ?? string.Empty;
        yield return City ?? string.Empty;
        yield return Name ?? string.Empty;
        yield return Zip ?? string.Empty;
        yield return IsBarred;
    }
}
