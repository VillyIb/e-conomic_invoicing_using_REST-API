using Eu.Iamia.Utils.Contract;
using Refit;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;

[Headers("accept: application/json")]
public partial interface IEconomicGatewayV2
{
    /// <summary>
    /// Read PamentTerms with paging
    /// </summary>
    /// <param name="skipPages"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    /// <see cref="DTO.PaymentTerms.get.PaymentTermsHandle"/>>
    /// <seealso cref="https://restdocs.e-conomic.com/#get-payment-terms"/>>
    /// <seealso cref="https://restapi.e-conomic.com/schema/payment-terms.get.schema.json"/>>
    [Get("/payment-terms?skippages={skipPages}&pagesize={pageSize}")]
    Task<DTO.PaymentTerms.get.PaymentTermsHandle> ReadPaymentTerms(int skipPages, int pageSize);


    /// <summary>
    /// Read customers with paging
    /// </summary>
    /// <param name="skipPages"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    /// <see cref="DTO.Customers.get.CustomersHandle"/>>
    /// <seealso cref="https://restdocs.e-conomic.com/#get-customers"/>>
    /// <seealso cref="https://restapi.e-conomic.com/schema/customers.get.schema.json"/>>
    [Get("/customers?skippages={skipPages}&pagesize={pageSize}")]
    Task<DTO.Customers.get.CustomersHandle> GetCustomers(int skipPages, int pageSize);


    /// <summary>
    /// Read Products with paging
    /// </summary>
    /// <param name="skipPages"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    /// <see cref="DTO.Products.get.ProductsHandle"/>>
    /// <seealso cref="https://restdocs.e-conomic.com/#get-products"/>>
    /// <seealso cref="https://restapi.e-conomic.com/schema/products.get.schema.json"/>>
    [Get("/products?skippages={skipPages}&pagesize={pageSize}")]
    Task<DTO.Products.get.ProductsHandle> ReadProducts(int skipPages, int pageSize);


    /// <summary>
    /// Reads Draft Invoices with paging
    /// </summary>
    /// <param name="skipPages"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    /// <see cref="DTO.Invoices.drafts.get.DraftInvoicesHandle"/>>
    /// <seealso cref="https://restdocs.e-conomic.com/#get-invoices-drafts"/>>
    /// <seealso cref="https://restapi.e-conomic.com/schema/invoices.drafts.get.schema.json"/>>
    [Get("/invoices/drafts")]
    Task<DTO.Invoices.drafts.get.DraftInvoicesHandle> GetDraftInvoices(int skipPages, int pageSize);


    /// <summary>
    /// Reads the Draft-Invoice by the specified <em>draftInvoiceNumber</em>
    /// </summary>
    /// <param name="draftInvoiceNumber"></param>
    /// <returns></returns>
    /// <see cref="DTO.Invoices.drafts.draftInvoiceNumber.get.DraftInvoice"/>>
    /// <seealso cref="https://restdocs.e-conomic.com/#get-invoices-drafts-draftinvoicenumber"/>>
    /// <seealso cref="https://restapi.e-conomic.com/schema/invoices.drafts.draftInvoiceNumber.get.schema.json"/>>
    [Get("/invoices/drafts/{draftInvoiceNumber}")]
    Task<DTO.Invoices.drafts.draftInvoiceNumber.get.DraftInvoice> GetDraftInvoice(int draftInvoiceNumber);

    /// <summary>
    /// Post the Draft-Invoice specified by <em>draftInvoice</em>
    /// </summary>
    /// <param name="draftInvoice"></param>
    /// <returns></returns>
    /// <see cref="DTO.Invoices.drafts.draftInvoiceNumber.lines.post.IDraftInvoice"/>>
    /// <see cref="DTO.Invoices.drafts.post.Invoice"/>>
    /// <seealso cref="https://restdocs.e-conomic.com/#post-invoices-drafts"/>>
    /// <seealso cref="https://restapi.e-conomic.com/schema/invoices.drafts.post.schema.json"/>>
    [Post("/invoices/drafts")]
    Task<DTO.Invoices.drafts.draftInvoiceNumber.lines.post.IDraftInvoice?> PostDraftInvoice(DTO.Invoices.drafts.post.Invoice draftInvoice);


    /// <summary>
    /// Read Booked-Invoices with paging and filtering by from-dat and to-date
    /// </summary>
    /// <param name="skipPages"></param>
    /// <param name="pageSize"></param>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <returns></returns>
    /// <see cref="DTO.Invoices.booked.get.BookedInvoicesHandle"/>>
    /// <seealso cref="https://restdocs.e-conomic.com/#get-invoices-booked"/>>
    /// <seealso cref="https://restapi.e-conomic.com/schema/invoices.booked.get.schema.json"/>>
    [Get("/invoices/booked?skippages={skipPages}&pagesize={pageSize}&filter=date$gte:{dateFrom}$and:date$lte:{dateTo}")]
    Task<DTO.Invoices.booked.get.BookedInvoicesHandle> ReadBookedInvoices(int skipPages, int pageSize, string dateFrom, string dateTo);

    /// <summary>
    /// Read Booked-Invoices with paging and filtering by Interval<DateTime>
    /// </summary>
    /// <param name="skipPages"></param>
    /// <param name="pageSize"></param>
    /// <param name="daterange"></param>
    /// <returns></returns>
    Task<DTO.Invoices.booked.get.BookedInvoicesHandle> ReadBookedInvoices(int skipPages, int pageSize, IInterval<DateTime> daterange)
            => ReadBookedInvoices(skipPages, pageSize, daterange.From.ToEconomicDate(), daterange.To.ToEconomicDate());

    /// <summary>
    /// Read Booked-Invoice by the specified <em>invoiceNumber</em>
    /// </summary>
    /// <param name="invoiceNumber"></param>
    /// <returns></returns>
    /// <see cref="DTO.Invoices.booked.bookedInvoiceNumber.get.BookedInvoice"/>>
    /// <seealso cref="https://restdocs.e-conomic.com/#get-invoices-booked-bookedinvoicenumber"/>>
    /// <seealso cref="https://restapi.e-conomic.com/schema/invoices.booked.bookedInvoiceNumber.get.schema.json"/>>
    [Get("/invoices/booked/{invoiceNumber}")]
    Task<DTO.Invoices.booked.bookedInvoiceNumber.get.BookedInvoice> GetBookedInvoice(int invoiceNumber);

}
