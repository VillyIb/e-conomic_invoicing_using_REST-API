﻿using Eu.Iamia.ConfigBase;
using Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Configuration;
public class Setup : IHandlerSetup
{
    private readonly IConfiguration _configuration;

    public Setup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private static void AddHandlers(IServiceCollection services)
    {
        services.AddTransient<IRestApiGateway, RestApiService>();
    }

    private void AddSettings(IServiceCollection services)
    {
        // see:https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-7.0
        // Binds Settings to the dependency injection container.
        services.Configure<SettingsForEConomicRestApi>(_configuration.GetSection(SettingsForEConomicRestApi.SectionName));
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
