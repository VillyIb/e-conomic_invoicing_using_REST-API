﻿using System.Diagnostics.CodeAnalysis;
using Eu.Iamia.ConfigBase;
using Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.IntegrationTests.Configuration;
public  class Setup : IHandlerSetup
{
    private readonly IConfiguration _configuration;

    public Setup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private static void AddHandlers(IServiceCollection services)
    {
        var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IEconomicGatewayV2));
        if (serviceDescriptor is null)
        {
            throw new ArgumentException("Order not OK");
        }
        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
        }
        services.AddTransient<IEconomicGatewayV2, GatewayV2TestVariant>();
    }

    private void AddSettings(IServiceCollection services)
    {
        // see:https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-7.0
        // Binds Settings to the dependency injection container.
        //services.Configure<SettingsForEConomicGatewayV2>(_configuration.GetSection(SettingsForEConomicGatewayV2.SectionName));
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