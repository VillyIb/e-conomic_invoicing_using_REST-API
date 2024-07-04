namespace Eu.Iamia.ConfigBase;

/// <summary>
/// Information shown when application is invoked with argument '--help'
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class HelpMetadataAttribute : Attribute
{
    public string Description { get; }

    public string ArgumentHelpName { get; }

    public bool Hide { get; }

    public bool IsRequired { get; init; }

    public HelpMetadataAttribute(string description, string argumentHelpName, bool isRequired = false, bool hide = false)
    {
        Description = description;
        ArgumentHelpName = argumentHelpName;
        Hide = hide;
        IsRequired = isRequired;
    }
}
