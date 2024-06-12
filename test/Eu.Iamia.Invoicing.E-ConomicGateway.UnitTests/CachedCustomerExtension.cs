using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public static class CachedCustomerExtension 
{
    public static CachedCustomer Valid()
    {
        return new CachedCustomer
        {
            CustomerNumber = 99999,
            Address = "Customer1 address",
            City = "Customer 1 city",
            Name = "Mocked Customer",
            PaymentTerms = 99999,
            Zip = "3390"
        };
    }
}