using System.Text.Json;

namespace Eu.Iamia.Utils;

public static class StringExtensions
{
    /// <summary>
    /// Returns the provided json with newlines and indenting.
    /// - or the provided NON-json as it is.
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static string JsonPrettify(this string json)
    {
        try
        {
            using var jDoc = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
        }
        catch
        {
            return json;
        }
    }
}
