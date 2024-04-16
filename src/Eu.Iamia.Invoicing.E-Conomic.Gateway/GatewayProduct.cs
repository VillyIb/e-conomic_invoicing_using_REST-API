namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    public async Task<string> ReadProductsPaged(int page, int pageSize)
    {
        try
        {
            SetAuthenticationHeaders();

            var response = await _httpClient.GetAsync($"https://restapi.e-conomic.com/products?skippages={page}&pagesize={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error("ReadProductsPaged", htmlBodyFail);

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
}
