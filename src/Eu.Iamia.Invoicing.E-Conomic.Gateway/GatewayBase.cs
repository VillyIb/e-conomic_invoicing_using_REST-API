using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Customer;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.DTO.Product;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Options;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;

public partial class GatewayBase : IEconomicGateway, IDisposable
{
    protected readonly ISerializerDraftInvoice SerializerDraftInvoice;
    protected readonly ISerializerCustomersHandle SerializerCustomersHandle;
    protected readonly ISerializerDeletedInvoices SerializerDeletedInvoices;
    protected readonly ISerializerProductsHandle SerializerProductsHandle;
    protected readonly ICustomerReport Report;
    protected readonly SettingsForEConomicGateway Settings;

    protected HttpClient? HttpClientField;

    protected virtual  HttpClient HttpClient => HttpClientField ??= new HttpClient();

    public GatewayBase(
    SettingsForEConomicGateway settings,
        ISerializerCustomersHandle serializerCustomersHandle,
        ISerializerDeletedInvoices serializerDeletedInvoices,
        ISerializerDraftInvoice serializerDraftInvoice,
        ISerializerProductsHandle serializerProductsHandle,
        ICustomerReport report
    )
    {
        Settings = settings;
        SerializerCustomersHandle = serializerCustomersHandle;
        SerializerDeletedInvoices = serializerDeletedInvoices;
        SerializerDraftInvoice = serializerDraftInvoice;
        SerializerProductsHandle = serializerProductsHandle;
        Report = report;
    }

    public GatewayBase(
        IOptions<SettingsForEConomicGateway> settings,
        ISerializerCustomersHandle serializerCustomersHandle,
        ISerializerDeletedInvoices serializerDeletedInvoices,
        ISerializerDraftInvoice serializerDraftInvoice,
        ISerializerProductsHandle serializerProductsHandle,
        ICustomerReport report
    ) : this(settings.Value, serializerCustomersHandle, serializerDeletedInvoices, serializerDraftInvoice, serializerProductsHandle, report)
    { }

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
        CheckToken(Settings.X_AgreementGrantToken, nameof(Settings.X_AgreementGrantToken));
        CheckToken(Settings.X_AppSecretToken, nameof(Settings.X_AppSecretToken));

        HttpClient.DefaultRequestHeaders.Remove("X-AppSecretToken");
        HttpClient.DefaultRequestHeaders.Add("X-AppSecretToken", Settings.X_AppSecretToken);

        HttpClient.DefaultRequestHeaders.Remove("X-AgreementGrantToken");
        HttpClient.DefaultRequestHeaders.Add("X-AgreementGrantToken", Settings.X_AgreementGrantToken);
    }

    private static async Task<string> GetHtmlBody(HttpResponseMessage response)
    {
        var htmlBody = await response.Content.ReadAsStringAsync();
        var h2 = htmlBody.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        return h2;
    }

    public void Dispose()
    {
        HttpClient.Dispose();
        Report.Dispose();
    }

    private CustomerCache? CustomerCache { get; set; }

    public async Task LoadCustomerCache(IList<int> customerGroupsToAccept)
    {
        CustomerCache = new CustomerCache(this, customerGroupsToAccept);
        await CustomerCache.LoadAllCustomers();
    }

    private ProductCache? ProductCache { get; set; }

    public async Task LoadProductCache()
    {
        ProductCache = new ProductCache(this);
        await ProductCache.LoadAllProducts();
    }
}
