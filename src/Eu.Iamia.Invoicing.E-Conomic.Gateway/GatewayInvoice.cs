using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.Loader.Contract;
using System.Text;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.DraftInvoice;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Mapping;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Utils;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    // see:http://restdocs.e-conomic.com/#post-invoices-drafts

    public async Task<string> ReadInvoice()
    {
        try
        {
            SetAuthenticationHeaders();

            var response = await _httpClient.GetAsync("https://restapi.e-conomic.com/invoices/drafts/340");
            response.EnsureSuccessStatusCode();
            var htmlBody = await GetHtmlBody(response);
            return htmlBody;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }

    // TODO return a more explicit status code !
    internal async Task<DraftInvoice?> PushInvoice(CachedCustomer customer, Invoice invoice, int sourceFileLineNumber)
    {
        try
        {
            SetAuthenticationHeaders();

            var json = invoice.ToJson();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://restapi.e-conomic.com/invoices/drafts", content);


            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                _report.Error("", htmlBodyFail);

                response.EnsureSuccessStatusCode();
            }

            var htmlBody = await GetHtmlBody(response);

            var draftInvoice = DraftInvoiceExtensions.FromJson(htmlBody);

            _report.Info("PushInvoice", htmlBody);

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
    }

    private Mapper? _mapper;

    private Mapper Mapper => _mapper ??= new Mapper(_settings, CustomerCache!, ProductCache!);

    public async Task<IDraftInvoice?> PushInvoice(IInputInvoice inputInvoice, int sourceFileLineNumber)
    {
        _report.SetCustomer(new CachedCustomer{Name = "---- ----", CustomerNumber = inputInvoice.CustomerNumber});

        try
        {
            var converted = Mapper.From(inputInvoice);

            var status = await PushInvoice(converted.customer, converted.ecInvoice, sourceFileLineNumber);
            return status;
        }
        catch (ApplicationException ex)
        {
            _report.Error("PushInvoice", ex.Message);

            return new DraftInvoice
            {
                DraftInvoiceNumber = -1,
                GrossAmount = 0.0
            };
        }
        finally
        {
            _report.Close(); 
        }
    }
}
