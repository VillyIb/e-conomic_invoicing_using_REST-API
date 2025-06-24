using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using Eu.Iamia.ConfigBase;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Configuration;

public static class AuthHeaderHandlerUtil
{
    public static void Replace(this HttpHeaders headers, string name, string value)
    {
        headers.Remove(name);
        headers.Add(name, value);
    }
}

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly Func<string> _secretToken;
    private readonly Func<string> _grantToken;

    public AuthHeaderHandler(Func<string> secretToken, Func<string> grantToken)
    {
        _secretToken = secretToken;
        _grantToken = grantToken;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Replace("X-AppSecretToken", _secretToken()); // TODO move name ...
        request.Headers.Replace("X-AgreementGrantToken", _grantToken()); // TODO move name ...

        return await base.SendAsync(request, cancellationToken);
    }
}

public class Setup : IHandlerSetup
{
    private readonly IConfiguration _configuration;

    public Setup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Func<string> GetTokenAsync(string key)
    {
        return () => _configuration.GetSection(SettingsForEConomicGatewayV2.SectionName).GetValue<string>(key) ?? $"Value for Key: '{key}' not found";
    }

    private void AddHandlers(IServiceCollection services)
    {
        var refitSettings = new RefitSettings();

        services.AddRefitClient<IEconomicGatewayV2>(refitSettings)
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://restapi.e-conomic.com")) // TODO move ...
            .AddHttpMessageHandler(
                () => new AuthHeaderHandler(
                    GetTokenAsync(nameof(SettingsForEConomicGatewayV2.X_AppSecretToken)),
                    GetTokenAsync(nameof(SettingsForEConomicGatewayV2.X_AgreementGrantToken))
                )
            )
        ;
    }

    private void AddSettings(IServiceCollection services)
    {
        services.Configure<SettingsForEConomicGatewayV2>(_configuration.GetSection(SettingsForEConomicGatewayV2.SectionName));
    }

    public void Register(IServiceCollection services)
    {
        AddSettings(services);
        AddHandlers(services);
    }

    [ExcludeFromCodeCoverage(Justification = "Unable to verify")]
    public void UnRegister()
    { }
}
