using Eu.Iamia.ConfigBase;
using static Eu.Iamia.ConfigBase.HelpMetadataAttribute;

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

    public int MaxErrors { get; set; } = 10;

    public int CustomerNumberLength { get; set; } = 4;

    public int CustomerNameLength { get; set; } = 4;

    public int CustomerSurnameLength { get; set; } = 4;

    /// <summary>
    /// True: Closing a file without errors in will be deleted.
    /// False: Files ar kept.
    /// </summary>
    [HelpMetadata("Only keep logfiles showing errors","true|false")]
    public bool DiscardNonErrorLogfiles { get; set; } = false;


}
