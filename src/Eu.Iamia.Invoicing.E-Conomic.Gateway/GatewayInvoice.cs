using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.Loader.Contract;
using System.Text;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Mapping;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;

// ReSharper disable InvalidXmlDocComment

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    // see:http://restdocs.e-conomic.com/#post-invoices-drafts

    internal async Task<IDraftInvoice> PushInvoice(CachedCustomer customer, Invoice invoice, int sourceFileLineNumber, CancellationToken cancellationToken)
    {
        const string reference = nameof(PushInvoice);

        SetAuthenticationHeaders();
        Report.SetCustomer(customer);

        var json = invoice.ToJson();
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await HttpClient.PostAsync("https://restapi.e-conomic.com/invoices/drafts", content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var htmlBodyFail = await GetHtmlBody(response);
            Report.Error(reference, htmlBodyFail);

            response.EnsureSuccessStatusCode();
        }

        const HttpStatusCode expected = HttpStatusCode.Created;
        if (expected != response.StatusCode)
        {
            var message = $@"Response status code does not indicate {expected}: {response.StatusCode:D} ({response.ReasonPhrase})";
            Report.Error(reference, message);
            throw new HttpRequestException(message, null, response.StatusCode);
        }

        var htmlBody = await GetHtmlBody(response);

        var draftInvoice = SerializerDraftInvoice.Deserialize(htmlBody);

        Report.Info(reference, htmlBody);

        return draftInvoice;
    }

    //internal async Task<string> GetDraftInvoices()
    //{
    //    const string reference = nameof(GetDraftInvoices);
    //    const string requestUri = $"https://restapi.e-conomic.com/invoices/drafts/";

    //    return await GetAny(requestUri, reference);
    //}

    //internal async Task<string> GetDraftInvoice(int invoiceNumber)
    //{
    //    const string reference = nameof(GetDraftInvoice);
    //    var requestUri = $"https://restapi.e-conomic.com/invoices/drafts/{invoiceNumber}";

    //    return await GetAny(requestUri, reference);
    //}

    [Obsolete]
    internal async Task<IDraftInvoice> GetDraftInvoiceY(int invoiceNumber)
    {
        const string reference = nameof(GetDraftInvoiceY);

        SetAuthenticationHeaders();

        var response = await HttpClient.GetAsync($"https://restapi.e-conomic.com/invoices/drafts/{invoiceNumber}");

        if (!response.IsSuccessStatusCode)
        {
            var htmlBodyFail = await GetHtmlBody(response);
            Report.Error(reference, htmlBodyFail);

            response.EnsureSuccessStatusCode();
        }

        const HttpStatusCode expected = HttpStatusCode.OK;
        if (expected != response.StatusCode)
        {
            var message = @"Response status code does not indicate {expected}: {response.StatusCode:D} ({response.ReasonPhrase})";
            Report.Error(reference, message);
            throw new HttpRequestException(message, null, response.StatusCode);
        }

        var htmlBody = await GetHtmlBody(response);

        var draftInvoice = SerializerDraftInvoice.Deserialize(htmlBody);

        Report.Info(reference, htmlBody);

        return draftInvoice;
    }

    // TODO wrap paging and return full content.

    //internal async Task<string> GetBookedInvoices
    //(
    //    int page,
    //    int pageSize,
    //    IInterval<DateTime> dateRange,
    //    CancellationToken cancellationToken
    //)
    //{
    //    // see: https://restdocs.e-conomic.com/#get-invoices-booked

    //    const string reference = nameof(GetBookedInvoices);

    //    // ReSharper disable once StringLiteralTypo
    //    var requestUri = $"https://restapi.e-conomic.com/invoices/booked?" +
    //                 $"skippages={page}&pagesize={pageSize}" +
    //                 $"&filter=" +
    //                 $"date$gte:{dateRange.From:yyyy-MM-dd}&date$lte:{dateRange.To:yyyy-Mm-dd}"
    //    ;
    //    return await GetAny(requestUri, reference);
    //}


    /// <summary>
    /// Delete single draft invoice
    /// </summary>
    /// <param name="invoiceNumber"></param>
    /// <returns></returns>
    /// <seealso cref="https://restdocs.e-conomic.com/#delete-invoices-drafts-draftinvoicenumber"/>>

    internal virtual async Task<string> DeleteDraftInvoiceTransport(int invoiceNumber)
    {
        const string reference = nameof(DeleteDraftInvoiceTransport);
        var requestUri = $"https://restapi.e-conomic.com/invoices/drafts/{invoiceNumber}";

        return await GetAny(requestUri, reference);
    }

    internal async Task<bool> DeleteDraftInvoice(int invoiceNumber)
    {
        var htmlBody = await DeleteDraftInvoiceTransport(invoiceNumber);

        /* Expected:
        {
           "message": "Deleted invoice."	
           ,"deletedCount": 1
           ,"deletedItems":
           [
           {
           "draftInvoiceNumber": 403
           ,"self": "https://restapi.e-conomic.com/invoices/drafts/403"
           }
           ]
           }
         */

        var deletedInvoices = SerializerDeletedInvoices.Deserialize(htmlBody);

        return deletedInvoices.deletedCount == 1
               &&
               deletedInvoices.deletedItems.Any(di => di.draftInvoiceNumber == invoiceNumber)
            ;
    }

    private Mapper? _mapper;
    private string _requestUri;
    //private string? requestUri;

    private Mapper Mapper => _mapper ??= new Mapper(Settings, CustomerCache!, ProductCache!);

    [Obsolete]
    public async Task<IDraftInvoice?> PushInvoice(IInputInvoice inputInvoice, int sourceFileLineNumber, CancellationToken cancellationToken)
    {
        Report.SetCustomer(new CachedCustomer { Name = "---- ----", CustomerNumber = inputInvoice.CustomerNumber });

        try
        {
            var converted = Mapper.From(inputInvoice);

            var status = await PushInvoice(converted.customer, converted.ecInvoice, sourceFileLineNumber, cancellationToken);
            return status;
        }
        finally
        {
            Report.Close();
        }
    }
}
