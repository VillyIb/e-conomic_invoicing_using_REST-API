using Eu.Iamia.ConfigBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

// ReSharper disable InconsistentNaming

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway.Configuration;
public class SettingsForEConomicGateway : SettingsBase
{
    public static string SectionName = "EConomicGateway";

    public SettingsForEConomicGateway() : base(SectionName) { }

    public string X_AppSecretToken { get; set; } = string.Empty;

    public string X_AgreementGrantToken { get; set; } = string.Empty;
    public IConfigurationSection GetSection(string key)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IConfigurationSection> GetChildren()
    {
        throw new NotImplementedException();
    }

    public IChangeToken GetReloadToken()
    {
        throw new NotImplementedException();
    }

    public string? this[string key]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public string  Key   { get; }
    public string  Path  { get; }
    public string? Value { get; set; }
}
