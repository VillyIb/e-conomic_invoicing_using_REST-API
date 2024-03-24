using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using Eu.Iamia.Invoicing.Loader.Contract;
using System.Text;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Mapping;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    // see:http://restdocs.e-conomic.com/#post-invoices-drafts

    public async Task<string> ReadInvoice()
    {
        try
        {
            SetAuthenticationHeaders();

            var response = await _httpClient.GetAsync("https://restapi.e-conomic.com/invoices/drafts/49");
            response.EnsureSuccessStatusCode();
            var htmlBody = await GetHtmlBody(response);
            return htmlBody;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }

    public async Task<string> PushInvoice(Invoice invoice)
    {
        try
        {
            SetAuthenticationHeaders();

            var json = invoice.ToJson();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://restapi.e-conomic.com/invoices/drafts", content);
            response.EnsureSuccessStatusCode();
            var htmlBody = await GetHtmlBody(response);
            return htmlBody;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }

    private Mapper? _mapper;

    private Mapper Mapper => _mapper ??=  new Mapper(CustomerCache, ProductCache);

    public async Task<string> PushInvoice(IInputInvoice invoice)
    {

        var economicInvoice = Mapper.From(invoice);

        if (economicInvoice == null)
        {
            throw new ArgumentException($"Unable to map invoice {invoice.CustomerNumber}");
        }

        var status = await PushInvoice(economicInvoice);
        return status;
    }

   
}
