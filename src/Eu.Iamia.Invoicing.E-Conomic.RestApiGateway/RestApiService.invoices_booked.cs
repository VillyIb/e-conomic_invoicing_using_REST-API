namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway;

using Eu.Iamia.Utils.Contract;

public partial class RestApiService
{
    public async Task<Stream> GetBookedInvoices
    (
        int skipPages,
        int pageSize,
        IInterval<DateTime> dateRange,
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#get-invoices-booked

        // see: https://restapi.e-conomic.com/schema/invoices.booked.get.schema.json

        const string reference = nameof(GetBookedInvoices);

        // ReSharper disable once StringLiteralTypo
        var requestUri = $"https://restapi.e-conomic.com/invoices/booked?" +
                         $"skippages={skipPages}&pagesize={pageSize}" +
                         $"&filter=" +
                         $"date$gte:{dateRange.From:yyyy-MM-dd}" +
                         $"$and:" +
                         $"date$lte:{dateRange.To:yyyy-MM-dd}"
            ;
        return await GetAsync(requestUri, reference, cancellationToken);
    }

    public async Task<Stream> GetBookedInvoice(
        int invoiceNumber, 
        CancellationToken cancellationToken
    )
    {
        // see: https://restdocs.e-conomic.com/#get-invoices-booked-bookedinvoicenumber

        // see: https://restapi.e-conomic.com/schema/invoices.booked.bookedInvoiceNumber.get.schema.json

        const string reference = nameof(GetBookedInvoice);

        var requestUri = $"https://restapi.e-conomic.com/invoices/booked/{invoiceNumber}";
        return await GetAsync(requestUri, reference, cancellationToken);
    }
}

