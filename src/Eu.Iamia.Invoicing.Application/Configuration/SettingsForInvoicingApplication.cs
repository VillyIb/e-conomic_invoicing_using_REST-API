using Eu.Iamia.ConfigBase;

namespace Eu.Iamia.Invoicing.Application.Configuration;

public class SettingsForInvoicingApplication : SettingsBase
{
    public static string SectionName => "Application";

    public SettingsForInvoicingApplication() : base(SectionName) { }

    [HelpMetadata(description:"Full path to .csv file", argumentHelpName: "path.csv")]
    public string CsvFile { get; set; } = string.Empty;
}
