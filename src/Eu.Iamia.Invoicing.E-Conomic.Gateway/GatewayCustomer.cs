namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase
{
    // https://restapi.e-conomic.com/customers?skippages=0&pagesize=20

    // TODO return Customer-collection, FromJson -> async.

    public async Task<string> ReadCustomersPaged(int page, int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            SetAuthenticationHeaders();

            var response = await _httpClient.GetAsync($"https://restapi.e-conomic.com/customers?skippages={page}&pagesize={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                var htmlBodyFail = await GetHtmlBody(response);
                Report.Error("ReadCustomersPaged", htmlBodyFail);

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
