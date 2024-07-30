using Eu.Iamia.Invoicing.Application.Contract.DTO;

namespace Eu.Iamia.Invoicing.Mapping.Caches;

public  class CustomerDtoCache : List<CustomerDto>
{
    public CustomerDto? GetCustomer(int customerNumber)
    {
        return this.FirstOrDefault(prod => prod.CustomerNumber == customerNumber);
    }
}
