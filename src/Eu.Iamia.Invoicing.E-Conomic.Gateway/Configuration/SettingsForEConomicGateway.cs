using Eu.Iamia.ConfigBase;

// ReSharper disable InconsistentNaming

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
public class SettingsForEConomicGateway : SettingsBase
{
    public static string SectionName = "EConomicGateway";

    public SettingsForEConomicGateway() : base(SectionName) { }

    public string X_AppSecretToken { get; set; } = string.Empty;

    public string X_AgreementGrantToken { get; set; } = string.Empty;

    public int PaymentTerms { get; set; } = 1;
}
