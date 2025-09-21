using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;

public interface IEconomicGatewayV2
{
    // TODO  GetCustomers(...)
    Task<DTO.Customers.get.CustomersHandle?> ReadCustomers(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );

    // TODO GetProducts
    Task<DTO.Products.get.ProductsHandle> ReadProducts(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );

    // TODO GetPaymentTerms
    Task<DTO.PaymentTerms.get.PaymentTermsHandle> ReadPaymentTerms(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );

    Task<DTO.Invoices.drafts.draftInvoiceNumber.lines.post.IDraftInvoice?> PostDraftInvoice(
        DTO.Invoices.drafts.post.Invoice restApiInvoice,
        int sourceFileNumber,
        CancellationToken cancellationToken = default
    );

    Task<DTO.Invoices.drafts.draftInvoiceNumber.lines.post.IDraftInvoice?> PostDraftInvoice(
        DTO.Invoices.drafts.post.Invoice draftInvoice,
        CancellationToken cancellationToken = default
    );

    Task<DTO.Invoices.drafts.get.DraftInvoicesHandle> GetDraftInvoices(
        int skipPages, 
        int pageSize,
        CancellationToken cancellationToken = default
    );

    Task<DTO.Invoices.drafts.draftInvoiceNumber.get.DraftInvoice?> GetDraftInvoice(
        int draftInvoiceNumber,
        CancellationToken cancellationToken = default
    );

    Task<string?> DeleteDraftInvoice(
        int draftInvoiceNumber,
        CancellationToken cancellationToken = default
    );

    // TODO GetBookedInvoices
    Task<DTO.Invoices.booked.get.BookedInvoicesHandle> ReadBookedInvoices(
        int page,
        int pageSize,
        IInterval<DateTime> dateRange,
        CancellationToken cancellationToken = default
    );

    // TODO GetBookedInvoice
    Task<DTO.Invoices.booked.bookedInvoiceNumber.get.BookedInvoice?> ReadBookedInvoice(
        int invoiceNumber,
        CancellationToken cancellationToken = default
    );
}
