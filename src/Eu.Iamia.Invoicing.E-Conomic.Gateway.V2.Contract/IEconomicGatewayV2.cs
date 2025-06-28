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
    [Get("/payment-terms?skippages={skipPages}&pagesize={pageSize}")]
    Task<PaymentTermsHandle> ReadPaymentTerms(int skipPages, int pageSize);


    [Get("/customers?skippages={skipPages}&pagesize={pageSize}")]
    Task<CustomersHandle> ReadCustomers(int skipPages, int pageSize);


    [Get("/products?skippages={skipPages}&pagesize={pageSize}")]
    Task<ProductsHandle> ReadProducts(int skipPages, int pageSize);


    [Get("/invoices/drafts")]
    Task<DTO.Invoices.drafts.get.DraftInvoicesHandle> ReadDraftInvoices(int skipPages, int pageSize);


    // see: https://restdocs.e-conomic.com/#get-invoices-drafts-draftinvoicenumber

    // see:https://restapi.e-conomic.com/schema/invoices.drafts.draftInvoiceNumber.get.schema.json

    [Get("/invoices/drafts/{draftInvoiceNumber}")]
    Task<DTO.Invoices.drafts.draftInvoiceNumber.get.DraftInvoice> GetDraftInvoice(int draftInvoiceNumber);

    [Post("/invoices/drafts")]
    Task<IDraftInvoice?> PostDraftInvoice(Invoice restApiInvoice);


    [Get("/invoices/booked?skippages={skipPages}&pagesize={pageSize}&filter=date$gte:{dateFrom}$and:date$lte:{dateTo}")]
    Task<Contract.DTO.Invoices.booked.get.BookedInvoicesHandle> ReadBookedInvoices(int skipPages, int pageSize, string dateFrom, string dateTo);

    Task<Contract.DTO.Invoices.booked.get.BookedInvoicesHandle> ReadBookedInvoices(int skipPages, int pageSize, IInterval<DateTime> daterange)
            => ReadBookedInvoices(skipPages, pageSize, daterange.From.ToEconomicDate(), daterange.To.ToEconomicDate());

    [Get("/invoices/booked/{invoiceNumber}")]
    Task<Contract.DTO.Invoices.booked.bookedInvoiceNumber.get.BookedInvoice> ReadBookedInvoice(int invoiceNumber);


}
