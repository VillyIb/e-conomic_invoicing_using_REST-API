namespace Eu.Iamia.ConfigBase;

public class HelpMetaData
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ArgumentHelpName { get; set; } = string.Empty;

    public bool IsRequired { get; set; } = false;

    public List<string> AliasList { get; set; } = [];

    public override string ToString()
    {
        return $"{Name} <{ArgumentHelpName}> {nameof(IsRequired)}: {IsRequired} \t{Description} ";
    }
}