using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eu.Iamia.ConfigBase;

public class MetadataUtil
{
    public List<HelpMetaData> HelpMetaData = new();

    // TODO not used?
    public void ExtractSettingsHelpMetaData (IServiceProvider serviceProvider, IServiceCollection services)
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
            var settingsDefinition = serviceProvider.GetService(typeof(IOptions<>).MakeGenericType(type));

            if (settingsDefinition is not IOptions<SettingsBase> settingsBase) continue;

            HelpMetaData.AddRange(settingsBase.Value.GetHelpMetadataList());
        }

        HelpMetaData = HelpMetaData.OrderBy(hm => hm.Name).ToList();
    }
}