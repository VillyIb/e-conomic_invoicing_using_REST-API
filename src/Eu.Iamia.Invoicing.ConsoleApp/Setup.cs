using Eu.Iamia.ConfigBase;
using Microsoft.Extensions.DependencyInjection;

namespace Eu.Iamia.Invoicing.ConsoleApp;

public  class Setup : SetupBase
{
    public Setup(string[]? args = null, IServiceCollection? services = null)
    {
        // NB! requires NuGet package MicrosoftExtensions.DependencyInjection
        // NB! requires NuGet package MicrosoftExtensions.Configuration.Json (implicit)

        var configuration = ConfigurationSetup.Init(args);

        services ??= new ServiceCollection();

        var businessLayerSetups = new IHandlerSetup[]
        {
            new Application.Configuration.Setup(configuration),
            new CSVLoader.Configuration.Setup(configuration),
            new E_Conomic.Gateway.Configuration.Setup(configuration),
            new E_Conomic.Gateway.V2.Configuration.Setup(configuration),
            new E_Conomic.RestApiGateway.Configuration.Setup(configuration),
            new Mapping.Configuration.Setup(configuration),
            new Reporting.Configuration.Setup(configuration),
        };

        Register(services,businessLayerSetups);

        ServiceProvider = services.BuildServiceProvider();

        ExtractSettingsHelpMetaData(services);
    }
}
