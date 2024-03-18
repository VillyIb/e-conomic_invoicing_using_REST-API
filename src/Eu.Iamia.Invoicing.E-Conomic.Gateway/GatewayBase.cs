using System.Net;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;
public partial class GatewayBase : IDisposable
{
    private readonly HttpClient _httpClient;

    public GatewayBase(HttpMessageHandler? httpClientHandler = null)
    {
        if (httpClientHandler is null)
        {
            httpClientHandler = new HttpClientHandler();
            ((HttpClientHandler)httpClientHandler).CookieContainer = new CookieContainer();
        }
        _httpClient = new HttpClient(httpClientHandler);
    }

    public void SetIdempotencyKey(string idempotencyKey)
    {
        _httpClient.DefaultRequestHeaders.Remove("Idempotency-Key");
        _httpClient.DefaultRequestHeaders.Add("Idempotency-Key", idempotencyKey);
    }

    public void SetDemoAuthenticationHeaders()
    {
        _httpClient.DefaultRequestHeaders.Remove("X-AppSecretToken");
        _httpClient.DefaultRequestHeaders.Add("X-AppSecretToken", "demo");

        _httpClient.DefaultRequestHeaders.Remove("X-AgreementGrantToken");
        _httpClient.DefaultRequestHeaders.Add("X-AgreementGrantToken", "demo");
    }
    
    private static async Task<string> GetHtmlBody(HttpResponseMessage response)
    {
        var stream = await response.Content.ReadAsStreamAsync();
        using var streamReader = new StreamReader(stream);
        var htmlBody = await streamReader.ReadToEndAsync();
        var h2 = htmlBody.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        return h2;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}

