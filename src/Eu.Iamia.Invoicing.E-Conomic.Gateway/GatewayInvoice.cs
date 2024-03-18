using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Invoice;
using System.Text;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    // see:http://restdocs.e-conomic.com/#post-invoices-drafts

    public async Task<string> ReadInvoice()
    {
        try
        {
            Set1660273AuthenticationHeaders();

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
            Set1660273AuthenticationHeaders();

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
}
