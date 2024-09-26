using Eu.Iamia.ConfigBase;

namespace Eu.Iamia.Invoicing.Application.Configuration;

public class SettingsForInvoicingApplication : SettingsBase
{
    public static string SectionName => "Application";

    public SettingsForInvoicingApplication() : base(SectionName) { }

    [HelpMetadata(description: "Full path to .csv file", argumentHelpName: "path.csv", false)]
    public string CsvFile { get; set; } = string.Empty;

    [HelpMetadata(description: "Gustomer goup numbers to accept", argumentHelpName: "E.g.: 1, 2, 3, 4, 5, 6, 11")]
    public List<int> CustomerGroupsToAccept { get; set; }
}
