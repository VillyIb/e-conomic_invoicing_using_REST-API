using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Reporting.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;


public class CachedCustomer : ICachedCustomer, ICustomer
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
}
