using Eu.Iamia.ConfigBase;
using Microsoft.Extensions.DependencyInjection;

namespace Eu.Iamia.Invoicing.Mapping.IntegrationTests;

public class Setup : SetupBase
{
    public Setup(string[]? args = null, IServiceCollection? services = null)
    {
        // NB! requires NuGet package MicrosoftExtensions.DependencyInjection
        // NB! requires NuGet package MicrosoftExtensions.Configuration.Json (implicit)

        var configuration = ConfigurationSetup.Init(args);

        services ??= new ServiceCollection();
        Register(
            services,
            new Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Configuration.Setup(configuration),
            new Eu.Iamia.Invoicing.Mapping.Configuration.Setup(configuration),
            new Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Configuration.Setup(configuration),
            new Eu.Iamia.Reporting.Configuration.Setup(configuration)
            );

        ServiceProvider = services.BuildServiceProvider();

        ExtractSettingsHelpMetaData(services);
    }
}
