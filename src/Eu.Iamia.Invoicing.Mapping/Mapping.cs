// ReSharper disable RedundantNameQualifier

namespace Eu.Iamia.Invoicing.Mapping;

/// <summary>
/// Mapping between Application data definition and RestApiData definition.
/// </summary>
public static class Mapping
{
    /// <summary>
    /// Incoming RestApi-Product to ProductDto.
    /// </summary>
    /// <param name="restApiProduct"></param>
    /// <returns></returns>
    public static Application.Contract.DTO.ProductDto ToProductDto(this E_Conomic.Gateway.V2.Contract.DTO.Product.Collection restApiProduct)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        var unitDto = restApiProduct.unit is null 
            ? null 
            : new Application.Contract.DTO.ProductDto.UnitDto
            {
                Name = restApiProduct.unit.name,
                UnitNumber = restApiProduct.unit.unitNumber
            }
        ;

        return new Application.Contract.DTO.ProductDto
        {
            Description = restApiProduct.description,
            ProductNumber = restApiProduct.productNumber,
            Name = restApiProduct.name,
            Unit = unitDto
        };
    }

    /// <summary>
    /// Incoming RestApi-Customer to CustomerDto.
    /// </summary>
    /// <param name="restApiCustomer"></param>
    /// <returns></returns>
    public static Application.Contract.DTO.CustomerDto ToCustomerDto(this E_Conomic.Gateway.V2.Contract.DTO.Customer.Collection restApiCustomer)
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

    /// <summary>
    /// Incoming RestApiPaymentTerm to PaymentTermDto.
    /// </summary>
    /// <param name="restApiPaymentTerm"></param>
    /// <returns></returns>
    public static Application.Contract.DTO.PaymentTermDto ToPaymentTermDto(this E_Conomic.Gateway.V2.Contract.DTO.PaymentTerm.Collection restApiPaymentTerm)
    {
        return new Application.Contract.DTO.PaymentTermDto()
        {
            DaysOfCredit = restApiPaymentTerm.daysOfCredit,
            Name = restApiPaymentTerm.name,
            PaymentTermNumber = restApiPaymentTerm.paymentTermsNumber.ToString(),
            PaymentTermsType = restApiPaymentTerm.paymentTermsType.ToString()
        };
    }
}
