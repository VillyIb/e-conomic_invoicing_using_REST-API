using System.Reflection;

namespace Eu.Iamia.ConfigBase;

public abstract class SettingsBase
{
    private readonly string _sectionName;

    public string GetSectionName() => _sectionName;

    protected SettingsBase(string sectionName)
    {
        _sectionName = sectionName;
    }

    // use reflection to get all instance properties
    public IList<HelpMetaData> GetHelpMetadataList()
    {
        var helpMetadataList = new List<HelpMetaData>();

        var properties = GetType().GetProperties();

        var readonlyProperties = properties.Where(p => !p.CanWrite).ToList();
        var rwProperties = properties.Where(p => p.CanWrite).ToList();

        var sectionNameProperty = readonlyProperties.FirstOrDefault(property => property.Name == "SectionName");
        var sectionName = sectionNameProperty?.GetValue(sectionNameProperty) as string ?? "<missing property: 'SectionName'>";

        foreach (var property in rwProperties)
        {
            if (!property.CanWrite) { continue; }

            var helpMetadata = property.GetCustomAttribute<HelpMetadataAttribute>();
            if (helpMetadata is null) { continue; }
            if (helpMetadata.Hide) continue;

            var helpMetaData = new HelpMetaData { Name = $"--{sectionName}:{property.Name}", Description = helpMetadata.Description, ArgumentHelpName = helpMetadata.ArgumentHelpName, IsRequired = helpMetadata.IsRequired};

            helpMetadataList.Add(helpMetaData);

            // todo read default value.
        }

        return helpMetadataList;
    }
}