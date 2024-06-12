using System.Text.Json;

namespace Eu.Iamia.Utils;

public static class StringExtensions
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    
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
            return JsonSerializer.Serialize(jDoc, Options);
        }
        catch
        {
            return json;
        }
    }


    public const char DefaultPostFix = '…';

    /// <summary>
    /// Cuts string at the end and pads with suffix at the end.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="postfix"></param>
    /// <returns></returns>
    public static string TrimToLength(this string? value, int length, char postfix = DefaultPostFix)
    {
        var t1 = value ?? string.Empty;

        var t2 = (t1.Length > length ? t1[..length] : t1).PadRight(length, postfix);

        return t2;
    }

    public const char DefaultPreFix = '_';

    /// <summary>
    /// Cuts number at the end and pads with prefix at the beginning.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public static string TrimNumberToLength(this string? value, int length, char prefix = DefaultPreFix)
    {
        var t1 = value ?? string.Empty;

        var t2 = (t1.Length > length ? t1[..length] : t1).PadLeft(length, prefix);

        return t2;
    }

    /// <summary>
    /// Returns a stream containing the specified value. Uses UTF8 encoding.
    /// Remember to dispose after user.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Stream GetStream(this string value)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream, System.Text.Encoding.UTF8);
        writer.Write(value);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

}
