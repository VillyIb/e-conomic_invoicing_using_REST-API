namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    // https://restapi.e-conomic.com/customers?skippages=0&pagesize=20

    public async Task<string> ReadCustomersPaged(int page, int pageSize)
    {
        try
        {
            SetAuthenticationHeaders();

            var response = await _httpClient.GetAsync($"https://restapi.e-conomic.com/customers?skippages={page}&pagesize={pageSize}");
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
