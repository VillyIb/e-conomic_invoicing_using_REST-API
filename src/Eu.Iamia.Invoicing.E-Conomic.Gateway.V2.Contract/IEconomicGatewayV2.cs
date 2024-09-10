using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Customers.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.Draft.Post;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.post;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.PaymentTerms.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Products.get;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;

public interface IEconomicGatewayV2
{
    Task<CustomersHandle> ReadCustomers(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<ProductsHandle> ReadProducts(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<PaymentTermsHandle?> ReadPaymentTerms(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<IDraftInvoice?> PostInvoice(Invoice restApiInvoice, int sourceFileNumber, CancellationToken cancellationToken);

    Task<int> LoadPaymentTermsCache();

    PaymentTerm? GetPaymentTerm(int paymentTermsNumber);
}
