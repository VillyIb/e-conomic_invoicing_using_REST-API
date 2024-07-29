namespace Eu.Iamia.Invoicing.Application.Contract.DTO;

public class CustomerDto : ValueObject<CustomerDto>
{
    public int CustomerNumber { get; init; }

    public int PaymentTerms { get; init; }

    public string? Address { get; init; }

    public string? City { get; init; }

    public string? Name { get; init; }

    public string? Zip { get; init; }

    public override string ToString()
    {
        return $"{nameof(CustomerNumber)}: {CustomerNumber}, {nameof(Name)}: {Name}, ";
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CustomerNumber;
        yield return PaymentTerms;
        yield return Address;
        yield return City;
        yield return Name;
        yield return Zip;

    }
}
