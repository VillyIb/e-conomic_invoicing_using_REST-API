using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Configuration;
using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using Microsoft.Extensions.Options;

using Refit;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway;

// apply to interface [Headers("accept: application/json", "Authorization: Bearer")]

public partial class RestApiService : IRestApiGateway
{
    protected readonly SettingsForEConomicRestApi Settings;
    //private readonly IRestMappingBase _restMappingBase;

    protected HttpClient? HttpClientField;

    protected virtual HttpClient HttpClient => HttpClientField ??= new HttpClient();

    public RestApiService(SettingsForEConomicRestApi settings)
    {
        Settings = settings;
    }

    public RestApiService(IOptions<SettingsForEConomicRestApi> settings) : this(settings.Value)
    { }

    private void SetAuthenticationHeaders()
    {
        HttpClient.DefaultRequestHeaders.Remove("X-AppSecretToken");
        HttpClient.DefaultRequestHeaders.Add("X-AppSecretToken", Settings.X_AppSecretToken);

        HttpClient.DefaultRequestHeaders.Remove("X-AgreementGrantToken");
        HttpClient.DefaultRequestHeaders.Add("X-AgreementGrantToken", Settings.X_AgreementGrantToken);
    }

    private static async Task<Stream> PostProcessing(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStreamAsync(cancellationToken);
        }

        var jsonError = await response.Content.ReadAsStringAsync(cancellationToken);

        throw new HttpRequestException(HttpRequestError.Unknown, jsonError, null, response.StatusCode);
    }

    internal async Task<Stream> GetAsync(string requestUri, string reference, CancellationToken cancellationToken)
    {
        SetAuthenticationHeaders();

        var response = await HttpClient.GetAsync(requestUri, cancellationToken);

        return await PostProcessing(response, cancellationToken);
    }

    internal async Task<Stream> PostAsync(string requestUri, StringContent content, string reference, CancellationToken cancellationToken)
    {
        SetAuthenticationHeaders();

        var response = await HttpClient.PostAsync(requestUri, content, cancellationToken);

        return await PostProcessing(response, cancellationToken);
    }

    internal async Task<Stream> DeleteAsync(string requestUri, string reference, CancellationToken cancellationToken)
    {
        SetAuthenticationHeaders();

        var response = await HttpClient.DeleteAsync(requestUri, cancellationToken);

        return await PostProcessing(response, cancellationToken);
    }
}
