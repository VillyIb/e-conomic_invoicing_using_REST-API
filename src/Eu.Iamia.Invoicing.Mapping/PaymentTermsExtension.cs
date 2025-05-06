using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.post;

namespace Eu.Iamia.Invoicing.Mapping;

public static class PaymentTermsExtension
{
    public static PaymentTerms ToPaymentTerms(this Application.Contract.DTO.PaymentTermDto paymentTerm)
    {
        return new PaymentTerms()
        {
            DaysOfCredit = paymentTerm.DaysOfCredit,
            Name = paymentTerm.Name,
            PaymentTermsNumber = int.Parse(paymentTerm.PaymentTermNumber),
            PaymentTermsType =
                Enum.Parse<PaymentTermsType>(paymentTerm
                    .PaymentTermsType)
        };
    }
}
