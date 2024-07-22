using Eu.Iamia.Utils.Contract;

// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.RestMapping;

public partial class RestMappingBase
{
    public async Task<Stream> GetDraftInvoices(
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#get-invoices-drafts

        const string reference = nameof(GetDraftInvoices);

        var requestUri =
            $"https://restapi.e-conomic.com/invoices/drafts?" +
            $"skippages={page}&pagesize={pageSize}"
        ;

        return await ExecuteRestApiCall(requestUri, reference, cancellationToken);
    }

    public async Task<Stream> GetDraftInvoice(
        int invoiceNumber,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#get-invoices-drafts-draftinvoicenumber

        const string reference = nameof(GetDraftInvoice);
        var requestUri = $"https://restapi.e-conomic.com/invoices/drafts/{invoiceNumber}";

        return await ExecuteRestApiCall(requestUri, reference, cancellationToken);
    }

    public async Task<Stream> GetBookedInvoices
    (
        int page,
        int pageSize,
        IInterval<DateTime> dateRange,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#get-invoices-booked

        const string reference = nameof(GetBookedInvoices);

        // ReSharper disable once StringLiteralTypo
        var requestUri = $"https://restapi.e-conomic.com/invoices/booked?" +
                         $"skippages={page}&pagesize={pageSize}" +
                         $"&filter=" +
                         $"date$gte:{dateRange.From:yyyy-MM-dd}&date$lte:{dateRange.To:yyyy-Mm-dd}"
            ;
        return await ExecuteRestApiCall(requestUri, reference, cancellationToken);
    }
}
