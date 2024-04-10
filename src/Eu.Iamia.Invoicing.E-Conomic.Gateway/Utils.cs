using System.Text.Json;

namespace Eu.Iamia.Invoicing.E_Conomic.Gateway;
public static class Utils
{
    public static string JsonPrettify(this string json)
    {
        using var jDoc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
    }
}
