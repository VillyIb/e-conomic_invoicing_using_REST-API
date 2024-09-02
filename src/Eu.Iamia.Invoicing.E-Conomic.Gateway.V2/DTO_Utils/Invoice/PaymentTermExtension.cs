namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.DTO_Utils.Invoice;

public static class PaymentTermExtension
{
    public static E_Conomic.Gateway.V2.Contract.DTO.Invoice.PaymentTerms ToInvoice(this E_Conomic.Gateway.V2.Contract.DTO.PaymentTerm.PaymentTerm paymentTerm)
    {
        return new E_Conomic.Gateway.V2.Contract.DTO.Invoice.PaymentTerms
        {
            DaysOfCredit = paymentTerm.daysOfCredit,
            Name = paymentTerm.name,
            PaymentTermsNumber = paymentTerm.paymentTermsNumber,
            PaymentTermsType =
                Enum.Parse<E_Conomic.Gateway.V2.Contract.DTO.Invoice.PaymentTermsType>(paymentTerm
                    .paymentTermsType)
        };
    }
}
