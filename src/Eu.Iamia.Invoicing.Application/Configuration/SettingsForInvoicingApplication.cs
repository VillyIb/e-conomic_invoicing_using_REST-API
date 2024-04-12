using Eu.Iamia.ConfigBase;

namespace Eu.Iamia.Invoicing.Application.Configuration;
public class SettingsForInvoicingApplication : SettingsBase
{
    public static string SectionName => "Application";

    public SettingsForInvoicingApplication() : base(SectionName) { }

    [HelpMetadata(description:"description", argumentHelpName: "full path filename")]
    public string CsvFileFullName { get; set; } = string.Empty;
}
