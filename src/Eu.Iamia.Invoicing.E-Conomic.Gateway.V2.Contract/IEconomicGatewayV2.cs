using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Customers.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.draftInvoiceNumber.lines.post;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.post;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.PaymentTerms.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Products.get;
using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;

public interface IEconomicGatewayV2
{
    Task<CustomersHandle> ReadCustomers(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<ProductsHandle> ReadProducts(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<PaymentTermsHandle?> ReadPaymentTerms(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<IDraftInvoice?> PostInvoice(Invoice restApiInvoice, int sourceFileNumber, CancellationToken cancellationToken);

    Task<Contract.DTO.Invoices.booked.get.BookedInvoicesHandle> ReadBookedInvoices(int page, int pageSize, IInterval<DateTime> dateRange, CancellationToken cancellationToken = default);

    Task<Contract.DTO.Invoices.booked.bookedInvoiceNumber.get.BookedInvoice> ReadBookedInvoice(int invoiceNumber, CancellationToken cancellationToken = default);

    Task<int> LoadPaymentTermsCache();

    PaymentTerm? GetPaymentTerm(int paymentTermsNumber);
}
