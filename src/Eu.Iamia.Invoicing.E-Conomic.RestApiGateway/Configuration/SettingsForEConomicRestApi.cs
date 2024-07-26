using Eu.Iamia.ConfigBase;

// ReSharper disable InconsistentNaming

namespace Eu.Iamia.Invoicing.E_Conomic.RestApiGateway.Configuration;
public class SettingsForEConomicRestApi() : SettingsBase(SectionName)
{
    public static string SectionName => "RestApiGateway";

    public string X_AppSecretToken { get; set; } = string.Empty;

    public string X_AgreementGrantToken { get; set; } = string.Empty;
}
