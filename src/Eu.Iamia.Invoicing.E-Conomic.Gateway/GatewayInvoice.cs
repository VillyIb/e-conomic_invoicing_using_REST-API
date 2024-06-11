using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.Loader.Contract;
using System.Text;
using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Mapping;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.DTO.DraftInvoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    // see:http://restdocs.e-conomic.com/#post-invoices-drafts

    // TODO return Invoice make FromJson Async.

    /// <summary>
    /// {HttpStatusCode.NoContent}
    /// </summary>
    private static readonly HttpStatusCode[] FailCodes = 
    {
        HttpStatusCode.NoContent
    };

    [Obsolete]
    public async Task<string> ReadInvoice()
    {
        const string reference = nameof(ReadInvoice);

        try
        {
            SetAuthenticationHeaders();

            var response = await HttpClient.GetAsync("https://restapi.e-conomic.com/invoices/drafts/340");

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error(reference, htmlBodyFail);

                response.EnsureSuccessStatusCode();
            }

            var htmlBody = await GetHtmlBody(response);
            return htmlBody;
        }
        catch (HttpRequestException ex)
        {
            return ex.StatusCode.ToString() ?? string.Empty;
        }
    }

    internal async Task<IDraftInvoice> PushInvoice(CachedCustomer customer, Invoice invoice, int sourceFileLineNumber)
    {
        const string reference = nameof(PushInvoice);

        try
        {
            SetAuthenticationHeaders();
            Report.SetCustomer(customer);

            var json = invoice.ToJson();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync("https://restapi.e-conomic.com/invoices/drafts", content);

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error(reference, htmlBodyFail);

                response.EnsureSuccessStatusCode();
            }

            if (FailCodes.Any(fc => fc == response.StatusCode))
            {
                Report.Info(reference, $"Response: {response.StatusCode}");
                return new FailedInvoice($"Response {response.StatusCode}");
            }

            var htmlBody = await GetHtmlBody(response);

            var draftInvoice = SerializerDraftInvoice.Deserialize(htmlBody);

            Report.Info(reference, htmlBody);

            return draftInvoice;
        }
        catch (HttpRequestException ex)
        {
            return new FailedInvoice(ex.Message);
        }
    }

    internal async Task<IDraftInvoice> GetDraftInvoice(int invoiceNumber)
    {
        const string reference = nameof(GetDraftInvoice);

        try
        {
            SetAuthenticationHeaders();

            var response = await HttpClient.GetAsync($"https://restapi.e-conomic.com/invoices/drafts/{invoiceNumber}");

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error(reference, htmlBodyFail);

                response.EnsureSuccessStatusCode();
            }

            if (FailCodes.Any(fc => fc == response.StatusCode))
            {
                Report.Info(reference, $"Response: {response.StatusCode}");
                return new FailedInvoice($"Response {response.StatusCode}");
            }

            var htmlBody = await GetHtmlBody(response);

            var draftInvoice = SerializerDraftInvoice.Deserialize(htmlBody);

            Report.Info(reference
                , htmlBody);

            return draftInvoice;
        }
        catch (HttpRequestException ex)
        {
            return new FailedInvoice(ex.Message);
        }
    }

    /// <summary>
    /// Delete single invoice
    /// </summary>
    /// <param name="invoiceNumber"></param>
    /// <returns></returns>
    /// <seealso cref="https://restdocs.e-conomic.com/#delete-invoices-drafts-draftinvoicenumber"/>>
    internal async Task<bool> DeleteInvoice(int invoiceNumber)
    {
        const string reference = nameof(DeleteInvoice);

        try
        {
            SetAuthenticationHeaders();

            var response = await HttpClient.DeleteAsync($"https://restapi.e-conomic.com/invoices/drafts/{invoiceNumber}");

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error(reference, htmlBodyFail);

                response.EnsureSuccessStatusCode();
            }

            if (FailCodes.Any(fc => fc == response.StatusCode))
            {
                Report.Info(reference, $"Response: {response.StatusCode}");
                return false;
            }

            var htmlBody = await GetHtmlBody(response);

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

            Report.Info(reference
                , htmlBody);

            return deletedInvoices.deletedCount == 1 
                   &&
                   deletedInvoices.deletedItems.Any(di => di.draftInvoiceNumber == invoiceNumber)
                ;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }

    private Mapper? _mapper;

    private Mapper Mapper => _mapper ??= new Mapper(Settings, CustomerCache!, ProductCache!);

    public async Task<IDraftInvoice?> PushInvoice(IInputInvoice inputInvoice, int sourceFileLineNumber)
    {
        const string reference = nameof(PushInvoice);

        Report.SetCustomer(new CachedCustomer { Name = "---- ----", CustomerNumber = inputInvoice.CustomerNumber });

        try
        {
            var converted = Mapper.From(inputInvoice);

            var status = await PushInvoice(converted.customer, converted.ecInvoice, sourceFileLineNumber);
            return status;
        }
        catch (ApplicationException ex)
        {
            Report.Error(reference, ex.Message);

            return new DraftInvoice
            {
                DraftInvoiceNumber = -1,
                GrossAmount = 0.0
            };
        }
        finally
        {
            Report.Close();
        }
    }
}
