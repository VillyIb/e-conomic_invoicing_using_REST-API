using System.Diagnostics.CodeAnalysis;
using Eu.Iamia.ConfigBase;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Contract.Serializers;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.Serializers;
using Eu.Iamia.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
public  class Setup : IHandlerSetup
{
    private readonly IConfiguration _configuration;

    public Setup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private static void AddHandlers(IServiceCollection services)
    {
        services.AddTransient<IEconomicGateway, GatewayBase>();
        services.AddSingleton<IJsonSerializerFacade, JsonSerializerFacade>();
        services.AddSingleton<ISerializerCustomersHandle, SerializerCustomersHandle>();
        services.AddSingleton<ISerializerDraftInvoice, SerializerDraftInvoice>();
        services.AddSingleton<ISerializerProductsHandle, SerializerProductsHandle>();
        services.AddSingleton<ISerializerDeletedInvoices, SerializerDeletedInvoices>();
    }

    private void AddSettings(IServiceCollection services)
    {
        // see:https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-7.0
        // Binds Settings to the dependency injection container.
        services.Configure<SettingsForEConomicGateway>(_configuration.GetSection(SettingsForEConomicGateway.SectionName));
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
