using System.Diagnostics.CodeAnalysis;
using Eu.Iamia.ConfigBase;
using Eu.Iamia.Reporting.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eu.Iamia.Reporting.Configuration;

public  class Setup : IHandlerSetup
{
    private readonly IConfiguration _configuration;

    public Setup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private static void AddHandlers(IServiceCollection services)
    {
        services.AddSingleton<ICustomerReport, CustomerCustomerReport>();
    }

    private void AddSettings(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
    {
        // see:https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-7.0
        // Binds Settings to the dependency injection container.
        services.Configure<SettingsForReporting>(_configuration.GetSection(SettingsForReporting.SectionName));
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
