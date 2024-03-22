using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace Eu.Iamia.ConfigBase;

public abstract class SetupBase : IDisposable
{
    // NB Requires NuGet package System.CommandLine
    // NB Requires NuGet package Microsoft.Extensions.Options.ConfigurationExtensions

    protected IServiceProvider? ServiceProvider;
    protected List<IHandlerSetup> SetupList = new();

    protected IServiceProvider GetServiceProvider() => ServiceProvider.CheckForNull($"{nameof(SetupBase)}.{nameof(GetServiceProvider)}");

    protected void Register(IServiceCollection services, params IHandlerSetup[] businessLayerSetups)
    {
        // see:https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-7.0
        // Binds Settings to the dependency injection container.

        foreach (var setup in businessLayerSetups)
        {
            SetupList.Add(setup);
            setup.Register(services);
        }

        services.AddOptions();
    }

    [ExcludeFromCodeCoverage(Justification = "Unable to verify")]
    protected void UnRegister()
    {
        foreach (var setup in SetupList)
        {
            setup.UnRegister();
        }
    }

    public T GetSetting<T>() where T : class, new()
    {
        var result = GetServiceProvider().GetService<IOptions<T>>();
        return result is null
            ? throw new ArgumentException($"Unsupported Type: '{typeof(T).Name}'")
            : result.Value;
    }

    public T GetService<T>()
    {
        var result = GetServiceProvider().GetService<T>();
        return result is null
            ? throw new ArgumentException($"Unsupported Type: '{typeof(T).Name}'")
            : result;
    }

    public List<HelpMetaData> HelpMetaData = new();

    /// <summary>
    /// Extracts all HelpMetadataAttributes defined on Configuration Properties.
    /// </summary>
    /// <param name="services"></param>
    protected void ExtractSettingsHelpMetaData(IServiceCollection services)
    {
        var sds = services.Where(
            sd =>
                sd.ServiceType.IsGenericType
                &&
                sd.ServiceType.GetGenericTypeDefinition() == typeof(IConfigureOptions<>)
        ).ToList();

        var types = sds.Select(s => s.ServiceType.GetGenericArguments()[0]).ToList();

        foreach (var type in types)
        {
            var settingsDefinition = GetServiceProvider().GetService(typeof(IOptions<>).MakeGenericType(type));

            if (settingsDefinition is not IOptions<SettingsBase> settingsBase) continue;

            HelpMetaData.AddRange(settingsBase.Value.GetHelpMetadataList());
        }

        HelpMetaData = HelpMetaData.OrderBy(hm => hm.Name).ToList();
    }

    /// <summary>
    /// Returns options generated from HelpMetaData on Configuration Properties.
    /// </summary>
    /// <returns></returns>
    public IList<Option> GetOptionList()
    {
        var result = new List<Option>();

        foreach (var helpMetadata in HelpMetaData)
        {
            var option = new Option<string>(name: helpMetadata.Name, description: helpMetadata.Description)
            {
                ArgumentHelpName = helpMetadata.ArgumentHelpName
            };

            foreach (var alias in helpMetadata.AliasList)
            {
                option.AddAlias(alias);
            }

            result.Add(option);
        }

        return result;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            UnRegister();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}