using Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
using System.Net;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Microsoft.Extensions.Options;
using Eu.Iamia.Reporting.Contract;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.RestMapping;

public partial class RestMappingBase : IRestMappingBase
{
    protected readonly SettingsForEConomicGateway Settings;
    protected readonly ICustomerReport Report;
    //private readonly IRestMappingBase _restMappingBase;

    protected HttpClient? HttpClientField;

    protected virtual HttpClient HttpClient => HttpClientField ??= new HttpClient();

    public RestMappingBase(
        SettingsForEConomicGateway settings,
        ICustomerReport report
        //,
        //IRestMappingBase restMappingBase // looks strange self..
        )
    {
        Settings = settings;
        Report = report;
        //_restMappingBase = restMappingBase;
    }

    public RestMappingBase(
        IOptions<SettingsForEConomicGateway> settings,
        ICustomerReport report
        //,
        //IRestMappingBase restMappingBase
    ) : this(settings.Value, report)
    { }

    /// <summary>
    /// Fails if token is null, whitespace or contain blanks.
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

    private void SetAuthenticationHeaders()
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

    private static async Task<Stream> GetPayload(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    private async Task<Stream> ExecuteRestApiCall(string requestUri, string reference, CancellationToken cancellationToken)
    {
        SetAuthenticationHeaders();

        var response = await HttpClient.GetAsync(requestUri, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var htmlBodyFail = await GetHtmlBody(response);
            Report.Error(reference, htmlBodyFail);

            response.EnsureSuccessStatusCode();
        }

        const HttpStatusCode expected = HttpStatusCode.OK;
        if (expected != response.StatusCode)
        {
            var message = @"Response status code does not indicate {expected}: {response.StatusCode:D} ({response.ReasonPhrase})";
            Report.Error(reference, message);
            throw new HttpRequestException(message, null, response.StatusCode);
        }

        return await GetPayload(response, cancellationToken);
    }
}
