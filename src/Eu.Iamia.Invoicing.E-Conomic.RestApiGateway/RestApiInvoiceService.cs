// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo

using Eu.Iamia.Utils.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway;

public partial class RestApiService
{
    public async Task<Stream> GetDraftInvoices(
        int skipPages,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#get-invoices-drafts

        const string reference = nameof(GetDraftInvoices);

        var requestUri =
            $"https://restapi.e-conomic.com/invoices/drafts?" +
            $"skippages={skipPages}&pagesize={pageSize}"
        ;

        return await GetAsync(requestUri, reference, cancellationToken);
    }

    public async Task<Stream> GetDraftInvoice(
        int invoiceNumber,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#get-invoices-drafts-draftinvoicenumber

        const string reference = nameof(GetDraftInvoice);
        var requestUri = $"https://restapi.e-conomic.com/invoices/drafts/{invoiceNumber}";

        return await GetAsync(requestUri, reference, cancellationToken);
    }

    public async Task<Stream> DeleteDraftInvoice(
        int invoiceNumber,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#delete-invoices-drafts

        const string reference = nameof(DeleteDraftInvoice);
        var requestUri = $"https://restapi.e-conomic.com/invoices/drafts/{invoiceNumber}";

        return await DeleteAsync(requestUri, reference, cancellationToken);
    }

    public async Task<Stream> GetBookedInvoices
    (
        int skipPages,
        int pageSize,
        IInterval<DateTime> dateRange,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#get-invoices-booked

        const string reference = nameof(GetBookedInvoices);

        // ReSharper disable once StringLiteralTypo
        var requestUri = $"https://restapi.e-conomic.com/invoices/booked?" +
                         $"skippages={skipPages}&pagesize={pageSize}" +
                         $"&filter=" +
                         $"date$gte:{dateRange.From:yyyy-MM-dd}&date$lte:{dateRange.To:yyyy-MM-dd}"
            ;
        return await GetAsync(requestUri, reference, cancellationToken);
    }

    public async Task<Stream> GetBookedInvoice(
        int invoiceNumber,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#get-invoices-booked-bookedinvoicenumber

        const string reference = nameof(GetBookedInvoices);

        var requestUri = $"https://restapi.e-conomic.com/invoices/booked/{invoiceNumber}";
        return await GetAsync(requestUri, reference, cancellationToken);
    }

    public async Task<Stream> PushInvoice(
        StringContent content,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#post-invoices-drafts-draftinvoicenumber-lines

        const string reference = nameof(PushInvoice);

        var requestUri = "https://restapi.e-conomic.com//invoices/drafts";
        return await PostAsync(requestUri, content, reference, cancellationToken);
    }
}
