namespace Eu.Iamia.Invoicing.Mapping;

public static class CustomerDtoExtension
{
    /// <summary>
    /// Incoming RestApi-Customer to CustomerDto.
    /// </summary>
    /// <param name="restApiCustomer"></param>
    /// <returns></returns>
    public static Application.Contract.DTO.CustomerDto ToCustomerDto(
        this E_Conomic.Gateway.V2.Contract.DTO.Customer.Customer restApiCustomer)
    {
        return new Application.Contract.DTO.CustomerDto
        {
            Name = restApiCustomer.name,
            Address = restApiCustomer.address,
            City = restApiCustomer.city,
            CustomerNumber = restApiCustomer.customerNumber,
            Zip = restApiCustomer.zip,
            PaymentTerms = restApiCustomer.paymentTerms.paymentTermsNumber
        };
    }
}