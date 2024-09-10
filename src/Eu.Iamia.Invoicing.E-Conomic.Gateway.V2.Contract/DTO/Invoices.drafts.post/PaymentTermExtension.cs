using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.PaymentTerms.get;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.post;

public static class PaymentTermExtension
{
    public static DTO.PaymentTerms ToInvoice(this PaymentTerm paymentTerm)
    {
        return new DTO.PaymentTerms
        {
            DaysOfCredit = paymentTerm.daysOfCredit,
            Name = paymentTerm.name,
            PaymentTermsNumber = paymentTerm.paymentTermsNumber,
            PaymentTermsType =
                Enum.Parse<PaymentTermsType>(paymentTerm
                    .paymentTermsType)
        };
    }
}
