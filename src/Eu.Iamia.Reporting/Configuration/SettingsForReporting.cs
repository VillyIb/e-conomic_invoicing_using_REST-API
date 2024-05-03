using Eu.Iamia.ConfigBase;

// ReSharper disable InconsistentNaming

namespace Eu.Iamia.Reporting.Configuration;

public class SettingsForReporting : SettingsBase
{
    public static string SectionName => "Reporting";

    public SettingsForReporting() : base(SectionName)
    { }

    public string OutputDirectory { get; set; } = string.Empty;

    public string TimePartFormat { get; set; } = "yyyy-MM-dd_hh-mm-ss";

    public string Filename { get; set; } = "InvoiceReport.txt";

    public int CustomerNumberLength { get; set; } = 4;

    public int CustomerNameLength { get; set; } = 4;

    public int CustomerSurnameLength { get; set; } = 4;

    /// <summary>
    /// True: When closing a file without errors in will be deleted.
    /// False: Files ar kept.
    /// </summary>
    [HelpMetadata("Remove log-files not showing errors","true|false")]
    public bool PruneLogfiles { get; set; } = false;


}
