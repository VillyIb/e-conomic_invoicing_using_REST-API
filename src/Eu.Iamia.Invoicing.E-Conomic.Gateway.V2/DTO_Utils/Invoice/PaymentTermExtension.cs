using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.post;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.PaymentTerms.get;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.DTO_Utils.Invoice;

public static class PaymentTermExtension
{
    public static PaymentTerms ToInvoice(this PaymentTerm paymentTerm)
    {
        return new PaymentTerms
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
