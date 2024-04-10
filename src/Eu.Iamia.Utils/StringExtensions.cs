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

    /// <summary>
    /// Cuts string at the end and pads with suffix at the end.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static string? TrimToLength(this string? value, int length, char suffix = '…')
    {
        var t1 = value ?? string.Empty;

        var t2 = (t1.Length > length ? t1[..length] : t1).PadRight(length, suffix);

        return t2;
    }

    /// <summary>
    /// Cuts number at the end and pads with prefix at the beginning.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public static string? TrimNumberToLength(this string? value, int length, char prefix = '_')
    {
        var t1 = value ?? string.Empty;

        var t2 = (t1.Length > length ? t1[..length] : t1).PadLeft(length, prefix);

        return t2;
    }

}
