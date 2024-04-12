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

