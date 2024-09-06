using Eu.Iamia.ConfigBase;
using Microsoft.Extensions.DependencyInjection;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.IntegrationTests;

public class Setup : SetupBase
{
    public Setup(string[]? args = null, IServiceCollection? services = null)
    {
        // NB! requires NuGet package MicrosoftExtensions.DependencyInjection
        // NB! requires NuGet package MicrosoftExtensions.Configuration.Json (implicit)

        var configuration = ConfigurationSetup.Init(args);

        services ??= new ServiceCollection();
        var  @params = new IHandlerSetup[] {
            new Invoicing.E_Conomic.Gateway.V2.Configuration.Setup(configuration), 
            new Reporting.Configuration.Setup(configuration), 
            new Invoicing.E_Conomic.RestApiGateway.Configuration.Setup(configuration), 
            new Invoicing.E_Conomic.Gateway.V2.IntegrationTests.Configuration.Setup(configuration)
        };

        Register(
            services,
            @params    
        );

        ServiceProvider = services.BuildServiceProvider();

        ExtractSettingsHelpMetaData(services);
    }
}
