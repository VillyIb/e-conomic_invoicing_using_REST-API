using Eu.Iamia.ConfigBase;
using Microsoft.Extensions.DependencyInjection;

namespace Eu.Iamia.Invoicing.E_ConomicGateway.UnitTests;

public  class Setup : SetupBase
{
    public Setup(string[]? args = null, IServiceCollection? services = null)
    {
        // NB! requires NuGet package MicrosoftExtensions.DependencyInjection
        // NB! requires NuGet package MicrosoftExtensions.Configuration.Json (implicit)

        var configuration = ConfigurationSetup.Init(args);

        services ??= new ServiceCollection();
        Register(
            services,
            new E_Conomic.Gateway.Configuration.Setup(configuration),
            new Reporting.Configuration.Setup(configuration)
        );

        ServiceProvider = services.BuildServiceProvider();

        ExtractSettingsHelpMetaData(services);
    }
}
