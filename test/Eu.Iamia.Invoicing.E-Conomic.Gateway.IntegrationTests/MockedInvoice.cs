using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.IntegrationTests;
public  class MockedInvoice : Invoice
{
    internal void SetPaymentTerms(int paymentTermsNumber)
    {
        PaymentTerms ??= new PaymentTerms();

        PaymentTerms.PaymentTermsNumber = paymentTermsNumber;
    }
}

