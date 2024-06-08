using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.Loader.Contract;
using System.Text;
using System.Text.Json;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.DraftInvoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Mapping;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Deserializers;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    // see:http://restdocs.e-conomic.com/#post-invoices-drafts

    // TODO return Invoice make FromJson Async.

    public async Task<string> ReadInvoice()
    {
        try
        {
            SetAuthenticationHeaders();

            var response = await _httpClient.GetAsync("https://restapi.e-conomic.com/invoices/drafts/340");

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error("ReadInvoice", htmlBodyFail);

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

    internal async Task<DraftInvoice?> PushInvoice(CachedCustomer customer, Invoice invoice, int sourceFileLineNumber)
    {
        try
        {
            SetAuthenticationHeaders();
            Report.SetCustomer(customer);

            var json = invoice.ToJson();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://restapi.e-conomic.com/invoices/drafts", content);

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error("PushInvoice", htmlBodyFail);

                response.EnsureSuccessStatusCode();
            }

            var htmlBody = await GetHtmlBody(response);

            var draftInvoice = new SerializerDraftInvoice(new JsonSerializerFacadeV2()).Deserialize(htmlBody);

            Report.Info("PushInvoice", htmlBody);

            return draftInvoice;
        }
        catch (HttpRequestException)
        {
            return new DraftInvoice
            {
                DraftInvoiceNumber = -1,
                GrossAmount = 0.0
            };
        }
        catch (Exception ex)
        {
            Report.Error("PushInvoice", ex.Message);
            return new DraftInvoice
            {
                DraftInvoiceNumber = -1,
                GrossAmount = 0.0
            };
        }
    }

    internal async Task<DraftInvoice> GetDraftInvoice(int invoiceNumber)
    {
        try
        {
            SetAuthenticationHeaders();

            var response = await _httpClient.GetAsync($"https://restapi.e-conomic.com/invoices/drafts/{invoiceNumber}");

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error("PushInvoice", htmlBodyFail);

                response.EnsureSuccessStatusCode();
            }

            var htmlBody = await GetHtmlBody(response);

            var draftInvoice = new SerializerDraftInvoice(new JsonSerializerFacadeV2()).Deserialize(htmlBody);

            Report.Info("PushInvoice", htmlBody);

            return draftInvoice;
        }
        catch (HttpRequestException)
        {
            return new DraftInvoice
            {
                DraftInvoiceNumber = -1,
                GrossAmount = 0.0
            };
        }
        catch (JsonException)
        {
            return new DraftInvoice
            {
                DraftInvoiceNumber = -1,
                GrossAmount = 0.0
            };
        }
    }

    internal async Task DeleteInvoce(int invoiceNumber)
    {
        // TODO implement.
    }

    private Mapper? _mapper;

    private Mapper Mapper => _mapper ??= new Mapper(Settings, CustomerCache!, ProductCache!);

    public async Task<IDraftInvoice?> PushInvoice(IInputInvoice inputInvoice, int sourceFileLineNumber)
    {
        Report.SetCustomer(new CachedCustomer { Name = "---- ----", CustomerNumber = inputInvoice.CustomerNumber });

        try
        {
            var converted = Mapper.From(inputInvoice);

            var status = await PushInvoice(converted.customer, converted.ecInvoice, sourceFileLineNumber);
            return status;
        }
        catch (ApplicationException ex)
        {
            Report.Error("PushInvoice", ex.Message);

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
