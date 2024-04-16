using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase : IEconomicGateway, IDisposable
{
    protected readonly ICustomerReport _report;
    protected readonly SettingsForEConomicGateway _settings;
    private readonly HttpClient _httpClient;

    public GatewayBase(IOptions<SettingsForEConomicGateway> settings, ICustomerReport report)
    {
        _report = report;
        _settings = settings.Value;
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// For UnitTesting.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="httpClientHandler"></param>
    internal GatewayBase(SettingsForEConomicGateway settings, ICustomerReport report, HttpMessageHandler httpClientHandler)
    {
        _settings = settings;
        _report = report;
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

    /// <summary>
    /// Fails if token is null, whitespace of contain blanks.
    /// </summary>
    /// <param name="token"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentException"></exception>
    private void CheckToken(string token, string name)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Please provide af value for ", name);
        }

        if (token.Contains(' '))
        {
            throw new ArgumentException($"Not a valid token '{token}'", name);
        }
    }

    protected void SetAuthenticationHeaders()
    {
        CheckToken(_settings.X_AgreementGrantToken, nameof(_settings.X_AgreementGrantToken));
        CheckToken(_settings.X_AppSecretToken, nameof(_settings.X_AppSecretToken));

        _httpClient.DefaultRequestHeaders.Remove("X-AppSecretToken");
        _httpClient.DefaultRequestHeaders.Add("X-AppSecretToken", _settings.X_AppSecretToken);

        _httpClient.DefaultRequestHeaders.Remove("X-AgreementGrantToken");
        _httpClient.DefaultRequestHeaders.Add("X-AgreementGrantToken", _settings.X_AgreementGrantToken);
    }

    private static async Task<string> GetHtmlBody(HttpResponseMessage response)
    {
        //var stream = await response.Content.ReadAsStreamAsync();
        //using var streamReader = new StreamReader(stream);
        //var htmlBody = await streamReader.ReadToEndAsync();
        var htmlBody = await response.Content.ReadAsStringAsync();
        var h2 = htmlBody.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        return h2;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private CustomerCache? CustomerCache { get; set; }

    public async Task LoadCustomerCache(IList<int> customerGroupsToAccept)
    {
        CustomerCache = new CustomerCache(this, customerGroupsToAccept);
        await CustomerCache.LoadAllCustomers();
    }

    private ProductCache? ProductCache { get; set; }

    public async Task LoadProcuctCache()
    {
        ProductCache = new ProductCache(this);
        await ProductCache.LoadAllProducts();
    }
}
