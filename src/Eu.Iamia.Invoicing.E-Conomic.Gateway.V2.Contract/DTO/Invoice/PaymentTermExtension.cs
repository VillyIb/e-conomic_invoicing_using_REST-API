using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoice;

public static class PaymentTermExtension
{
    public static E_Conomic.Gateway.V2.Contract.DTO.Invoice.PaymentTerms ToInvoice(this E_Conomic.Gateway.V2.Contract.DTO.PaymentTerm.Collection paymentTerm)
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
