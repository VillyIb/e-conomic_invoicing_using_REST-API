﻿namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.post;

public static class PaymentTermExtension
{
    public static PaymentTerms ToInvoice(this DTO.PaymentTerms.get.PaymentTerm paymentTerm)
    {
        return new PaymentTerms()
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
