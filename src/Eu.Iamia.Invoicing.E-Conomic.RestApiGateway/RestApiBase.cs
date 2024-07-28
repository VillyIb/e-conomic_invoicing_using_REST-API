using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using Microsoft.Extensions.Options;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway;

public partial class RestApiBase : IRestApiGateway
{
    protected readonly SettingsForEConomicRestApi Settings;
    //private readonly IRestMappingBase _restMappingBase;

    protected HttpClient? HttpClientField;

    protected virtual HttpClient HttpClient => HttpClientField ??= new HttpClient();

    public RestApiBase(SettingsForEConomicRestApi settings)
    {
        Settings = settings;
    }

    public RestApiBase(IOptions<SettingsForEConomicRestApi> settings) : this(settings.Value)
    { }

    private void SetAuthenticationHeaders()
    {
        HttpClient.DefaultRequestHeaders.Remove("X-AppSecretToken");
        HttpClient.DefaultRequestHeaders.Add("X-AppSecretToken", Settings.X_AppSecretToken);

        HttpClient.DefaultRequestHeaders.Remove("X-AgreementGrantToken");
        HttpClient.DefaultRequestHeaders.Add("X-AgreementGrantToken", Settings.X_AgreementGrantToken);
    }

    private static async Task<Stream> GetPayload(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    private async Task<Stream> GetAsync(string requestUri, string reference, CancellationToken cancellationToken)
    {
        SetAuthenticationHeaders();

        var response = await HttpClient.GetAsync(requestUri, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await GetPayload(response, cancellationToken);
    }

    private async Task<Stream> PostAsync(string requestUri, StringContent content, string reference, CancellationToken cancellationToken)
    {
        SetAuthenticationHeaders();

        var response = await HttpClient.PostAsync(requestUri, content, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await GetPayload(response, cancellationToken);
    }

}
