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

public record EConomicAuthentication(string X_appSecretToken, string X_agreementGrantToken);

public class AuthHeaderHandler(Func<EConomicAuthentication> authSettingDelegate) : DelegatingHandler
{
    private readonly Func<EConomicAuthentication> _authSettingDelegate = authSettingDelegate;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        EConomicAuthentication authSetting = _authSettingDelegate();

        request.Headers.Replace("X-AppSecretToken", authSetting.X_appSecretToken);
        request.Headers.Replace("X-AgreementGrantToken", authSetting.X_agreementGrantToken);

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

    private void CheckForNullOrWhitespace(Func<string> name, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new NullReferenceException($"appsettings[{_configuration.GetSection("COMPUTERNAME").Value}].json: '{SettingsForEConomicGatewayV2.SectionName}.{name()}' is null or whitespace");
        }
    }

    private Func<EConomicAuthentication> GetAuthentication()
    {
        SettingsForEConomicGatewayV2 economicGatewaySettings = new();
        _configuration.GetSection(SettingsForEConomicGatewayV2.SectionName).Bind(economicGatewaySettings);

        CheckForNullOrWhitespace(
            () => nameof(economicGatewaySettings.X_AppSecretToken),
            economicGatewaySettings.X_AppSecretToken
        );
        CheckForNullOrWhitespace(
            () => nameof(economicGatewaySettings.X_AgreementGrantToken),
            economicGatewaySettings.X_AgreementGrantToken
        );

        return () => new EConomicAuthentication(
            economicGatewaySettings.X_AppSecretToken,
            economicGatewaySettings.X_AgreementGrantToken
        );
    }

    private void AddHandlers(IServiceCollection services)
    {
        var refitSettings = new RefitSettings();

        services.AddRefitClient<IEconomicGatewayV2>(refitSettings)
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://restapi.e-conomic.com")) // TODO move ...
            .AddHttpMessageHandler(
                () => new AuthHeaderHandler(
                    GetAuthentication()
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
