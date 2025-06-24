using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Customers.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.draftInvoiceNumber.lines.post;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Invoices.drafts.post;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.PaymentTerms.get;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract.DTO.Products.get;
using Eu.Iamia.Utils.Contract;
using Refit;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;

[Headers("accept: application/json")]
public interface IEconomicGatewayV2
{
    [Get("/customers?skippages={skipPages}&pagesize={pageSize}")]
    Task<CustomersHandle> ReadCustomers(int skipPages, int pageSize);

    [Get("/products?skippages={skipPages}&pagesize={pageSize}")]
    Task<ProductsHandle> ReadProducts(int skipPages, int pageSize);

    [Post("/invoices/drafts")]
    Task<IDraftInvoice?> PostDraftInvoice(Invoice restApiInvoice);

    [Get("/invoices/booked?skippages={skipPages}&pagesize={pageSize}&filter=date$gte:{dateFrom}$and:date$lte:{dateTo}")]
    Task<Contract.DTO.Invoices.booked.get.BookedInvoicesHandle> ReadBookedInvoices(int skipPages, int pageSize, string dateFrom, string dateTo);

    Task<Contract.DTO.Invoices.booked.get.BookedInvoicesHandle> ReadBookedInvoices(int skipPages, int pageSize, IInterval<DateTime> daterange)
            => ReadBookedInvoices(skipPages, pageSize, daterange.From.ToEconomicDate(), daterange.To.ToEconomicDate());

    [Get("/invoices/drafts/booked/{invoiceNumber}")]
    Task<Contract.DTO.Invoices.booked.bookedInvoiceNumber.get.BookedInvoice> ReadBookedInvoice(int invoiceNumber);

    [Get("/payment-terms?skippages={skipPages}&pagesize={pageSize}")]
    Task<PaymentTermsHandle> ReadPaymentTerms(int skipPages, int pageSize);

    //// TODO Consider call it GetCustomers(...)
    //Task<CustomersHandle> ReadCustomers(
    //    int page,
    //    int pageSize,
    //    CancellationToken cancellationToken = default
    //);

    //Task<ProductsHandle> ReadProducts(
    //    int page,
    //    int pageSize,
    //    CancellationToken cancellationToken = default
    //);

    //Task<PaymentTermsHandle> ReadPaymentTerms(
    //    int page,
    //    int pageSize,
    //    CancellationToken cancellationToken = default
    //);

    //Task<IDraftInvoice?> PostDraftInvoice(
    //    Invoice restApiInvoice,
    //    int sourceFileNumber,
    //    CancellationToken cancellationToken
    //);

    //Task<Contract.DTO.Invoices.booked.get.BookedInvoicesHandle> ReadBookedInvoices(
    //    int page,
    //    int pageSize,
    //    IInterval<DateTime> dateRange,
    //    CancellationToken cancellationToken = default
    //);

    //Task<Contract.DTO.Invoices.booked.bookedInvoiceNumber.get.BookedInvoice?> ReadBookedInvoice(
    //    int invoiceNumber,
    //    CancellationToken cancellationToken = default
    //);
}
