using Eu.Iamia.ConfigBase;

// ReSharper disable InconsistentNaming

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.V2.Configuration;
public class SettingsForEConomicGatewayV2 : SettingsBase
{
    public static string SectionName => "EConomicGatewayV2";

    public SettingsForEConomicGatewayV2() : base(SectionName) { }

    public string X_AppSecretToken { get; set; } = string.Empty;

    public string X_AgreementGrantToken { get; set; } = string.Empty;

    public int PaymentTerms { get; set; } = 1;

    public int LayoutNumber { get; set; } = 21;
}
