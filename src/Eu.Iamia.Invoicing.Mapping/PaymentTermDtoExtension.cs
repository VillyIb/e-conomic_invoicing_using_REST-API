namespace Eu.Iamia.Invoicing.Mapping;

public static class PaymentTermDtoExtension
{
    /// <summary>
    /// Incoming RestApiPaymentTerm to PaymentTermDto.
    /// </summary>
    /// <param name="restApiPaymentTerm"></param>
    /// <returns></returns>
    public static Application.Contract.DTO.PaymentTermDto ToPaymentTermDto(this E_Conomic.Gateway.V2.Contract.DTO.PaymentTerm.PaymentTerm restApiPaymentTerm)
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