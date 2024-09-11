using System.Diagnostics.CodeAnalysis;
using Eu.Iamia.ConfigBase;
using Eu.Iamia.Invoicing.Application.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eu.Iamia.Invoicing.Application.Configuration;
public class Setup : IHandlerSetup
{
    private readonly IConfiguration _configuration;

    public Setup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private static void AddHandlers(IServiceCollection services)
    {
        services.AddTransient<IInvoicingHandler, InvoicingHandler>();
        services.AddTransient<IExportService, ExportService>();
    }

    private void AddSettings(IServiceCollection services)
    {
        // see:https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-7.0
        // Binds Settings to the dependency injection container.
        services.Configure<SettingsForInvoicingApplication>(_configuration.GetSection(SettingsForInvoicingApplication.SectionName));
    }

    public void Register(IServiceCollection services)
    {
        AddSettings(services);
        AddHandlers(services);
    }

    [ExcludeFromCodeCoverage(Justification = "Unable to verify")]
    public void UnRegister()
    {

    }
}
