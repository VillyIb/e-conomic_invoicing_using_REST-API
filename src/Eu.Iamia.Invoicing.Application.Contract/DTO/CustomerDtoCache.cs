namespace Eu.Iamia.Invoicing.Application.Contract.DTO;

public  class CustomerDtoCache : List<CustomerDto>
{
    public CustomerDto? GetCustomer(int customerNumber)
    {
        return this.FirstOrDefault(prod => prod.CustomerNumber == customerNumber);
    }
}
